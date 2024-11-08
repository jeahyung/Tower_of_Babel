using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Artngame.PDM {

public class ParticleCollisionsPDM : MonoBehaviour {
	
		//v2.3
		public int maxCollisions = 10;//remove extra collisions if needed
		public int maxCollisions2 = 10;

		//v2.1
		//public bool Cycle_pools = false; //cycle between other pool systems to use upon collisions
		public bool normal_On_Y = true;//if connected to propagator, this is ovverriden
		public bool Decal_per_mat = false;//override pool gameobject, with a list of gameobjects to use as decals per material name
		public List<string> Mat_names = new List<string>();

		public List<Transform> Decals_for_mat = new List<Transform>();//decals to use for each different material name
		//use "Any" as material name to cast specified decal to everything else, otherwise no decals will appear outside 
		//the chosen material names
		public List<ParticlePropagationPDM> Add_effects = new List<ParticlePropagationPDM>();

	void Start () {
			p11 = this.gameObject.GetComponent<ParticleSystem>();
			p11Transform = this.gameObject.transform;

			if(ParticlePOOL!=null){
				Propagator = ParticlePOOL.GetComponent(typeof(ParticlePropagationPDM)) as ParticlePropagationPDM;
			}

			collisionEvents = new ParticleCollisionEvent[1][]; 
	
			if(Gameobj_instances !=null){

				Gameobj_instances.Clear();

			}

			systems_To_override = new List<ParticlePropagationPDM>();

			if(Systems_To_override !=null){
				if(Systems_To_override.Count >0){
					for(int i=0;i< Systems_To_override.Count;i++){
						systems_To_override.Add(Systems_To_override[i].GetComponent(typeof(ParticlePropagationPDM)) as ParticlePropagationPDM);					
					}
				}
			}		
	}

		public GameObject ParticlePOOL;
		private ParticlePropagationPDM Propagator;
		private ParticleCollisionEvent[][] collisionEvents;

		public float Flame_force=0.5f;
		private Transform p11Transform;

		//overrides
		public List<GameObject> Systems_To_override;
		private List<ParticlePropagationPDM> systems_To_override;//use to cache scripts
		public float cut_off_dist=0.1f; //max distance to check other system for painted positions

		void Update(){

			//particle-particle collision
			if(inner_particle_collision){

			if(p11 == null){
				p11 = this.gameObject.GetComponent<ParticleSystem>();
				p11Transform = this.gameObject.transform;
			}

					ParticleList = new ParticleSystem.Particle[p11.particleCount];
					p11.GetParticles(ParticleList);

					int counter=0;
					for (int i=0; i < ParticleList.Length;i=i+divide_factor)
					{
						//emit colliders until reach max
						if(Gameobj_instances.Count < (ParticleList.Length / divide_factor)){
							
							GameObject TEMP = Instantiate(Collider_OBJ,   ParticleList[i].position   ,Quaternion.identity)as GameObject;
							
							Gameobj_instances.Add(TEMP);
							TEMP.transform.position = ParticleList[i].position;
							
							TEMP.transform.parent = this.gameObject.transform;

						}
						if(counter < Gameobj_instances.Count){
							Gameobj_instances[counter].transform.position = ParticleList[i].position;

							if(end_of_life){

								if(ParticleList[i].remainingLifetime < life_factor_upper*ParticleList[i].startLifetime
								   	& ParticleList[i].remainingLifetime > life_factor_lower*ParticleList[i].startLifetime
								   	& Vector3.Distance(ParticleList[i].position,p11Transform.position) > min_source_dist
									& p11.emission.enabled ///v2.1 & p11.enableEmission
								   ){
									Gameobj_instances[counter].GetComponent<Collider>().enabled=true;
								}else{
									Gameobj_instances[counter].GetComponent<Collider>().enabled=false;
								}
							}
						}
						counter++;
					}
			}
		}

		//particle-particle collision - choose target particle to search for, if close emit collider objects until particle dies
		public bool inner_particle_collision = false;
		public GameObject thrower_to_check;
		public GameObject Collider_OBJ;
		public List<GameObject> Gameobj_instances; //collider instances kept here
		//public float min_check_dist=10f;

		public int divide_factor = 10; //devide particles to get fewer colliders out
		private ParticleSystem.Particle[] ParticleList;
		public ParticleSystem p11;
		public bool end_of_life=false;//enable collider only near particle death
		public float life_factor_upper=0.8f;//at which point collider is enabled, 0.1f = at 90% of particle life
		public float life_factor_lower=0.1f;
		public float min_source_dist = 5f;

		public bool is_fire=false;
		public bool is_ice=false;
		public bool enable_LocalWind=false;
		public bool enable_flyaway=false;

		public bool Overide_Color=false;
		public Color New_Color=Color.black;

	void OnParticleCollision(GameObject other){

			//v2.0
			if(collisionEvents == null){
				Start ();
				//collisionEvents = new ParticleSystem.CollisionEvent[1][];
			}

			bool Procedural_check = false;

			if(Propagator !=null){						

			if(Propagator.Grow_ice_mesh){

				//search for it
				if(other.tag =="Flammable"){//if collider has tag, seach from grow script

					Ice_Grow_PDM ICE_GROWTH = other.GetComponentInChildren(typeof(Ice_Grow_PDM)) as Ice_Grow_PDM;

					ProceduralNoisePDM MESH_NOISE = other.GetComponentInChildren(typeof(ProceduralNoisePDM)) as ProceduralNoisePDM;

					if(ICE_GROWTH!=null){
						if(ICE_GROWTH.is_ice == Propagator.is_ice){
							ICE_GROWTH.trigger_ice_grow=true;
						}
					}

						if(MESH_NOISE !=null){
							if(MESH_NOISE.enabled){
								Procedural_check=true;
							} 
						}

				}

			}

			}//END if propagtor exists


			bool propagator_exists = true;
			if(Propagator == null){

				propagator_exists=false;
			}

			bool Propagator_use_collisions=false;
			if(Propagator != null){
				
				Propagator_use_collisions=Propagator.use_particle_collisions;
			}

						
			//if( (propagator_exists & Propagator_use_collisions & !Procedural_check) | !propagator_exists ){
			if(((propagator_exists & Propagator_use_collisions & !Procedural_check) | !propagator_exists ) ){

				bool layer_same=false;
				if(propagator_exists){
					for(int i=0;i<Propagator.Layers.Count;i++){
						int AA = LayerMask.NameToLayer(Propagator.Layers[i]);
						if(other.layer == AA){
							layer_same=true;
						}
					}
				}

				bool Propagator_is_fire=false;
				bool Propagator_is_ice=false;
				bool Propagator_by_layer=false;
				if(propagator_exists){
					Propagator_is_fire=Propagator.is_fire;
					Propagator_is_ice=Propagator.is_ice;
					Propagator_by_layer=Propagator.By_layer;
				}

				if((propagator_exists &&(!Propagator_by_layer ||(Propagator_by_layer & layer_same ))) || !propagator_exists){
					
					if((propagator_exists &&(!( (Propagator_is_fire || this.is_fire) && other.layer == LayerMask.NameToLayer("FireParticlesCollision")) 
					                         && !((Propagator_is_ice|| this.is_ice) && other.layer == LayerMask.NameToLayer("IceParticlesCollision"))
					                         )) || !propagator_exists)
					{


				
				int particle_count = 0;
				if(propagator_exists){
					particle_count = Propagator.p11.main.maxParticles; //v2.3
				}

				//get collisions from flamthrower particle system, add flame points there
				
				for(int j=0; j<collisionEvents.Length;j++){					

						collisionEvents[j] = new ParticleCollisionEvent[ p11.GetSafeCollisionEventSize()];
					
				}
				for(int j=0; j<collisionEvents.Length;j++){					

						//v3.4.6 SM - v2.3 PDM
						List<ParticleCollisionEvent> CollisionEventsList = new List<ParticleCollisionEvent>();
						CollisionEventsList.AddRange(collisionEvents[j]);
						p11.GetCollisionEvents(other,CollisionEventsList);
						collisionEvents[j] = CollisionEventsList.ToArray();						
						//p11.GetCollisionEvents(other,collisionEvents[j]);

				}

				//v2.3
				int counter1 = collisionEvents.Length;
				if( counter1 > maxCollisions){
					counter1 = maxCollisions;
				}

				for (int j = 0; j < counter1; j++) //v2.3 - Unity 5.6 issue handle, dual contacts and blood decals issue
                {

							//v2.3
							int counter2 = collisionEvents[j].Length;
							if( counter2 > maxCollisions2){
								counter2 = maxCollisions2;
							}

                            //for(int j=0; j<collisionEvents.Length;j++){
							for (int k = 0; k < counter2; k++){ //v2.3 - Unity 5.6 issue handle, dual contacts and blood decals issue
                            //for (int k=0; k<collisionEvents[j].Length;k++){

							//overrides

							if(systems_To_override!=null){
								for(int i=0;i< systems_To_override.Count;i++){									
								
									if(systems_To_override[i].enable_overides){
										for(int m=0;m<systems_To_override[i].Registered_paint_positions.Count;m++){

											if(Vector3.Distance(collisionEvents[j][k].intersection,systems_To_override[i].Registered_paint_positions[m]) < cut_off_dist)
											{												

													if(Overide_Color){
														systems_To_override[i].Particle_color[m]=New_Color;
													}

												//1 is for fire vs ice and vise versa
												//2 is for fly away
												//3 is for fly away AND melt (1+2)
												//4 local_wind
												if(propagator_exists){
													if(Propagator.is_fire & systems_To_override[i].is_ice){
														systems_To_override[i].LocalOverrides[m]=1;
													}
													if(Propagator.is_ice & systems_To_override[i].is_fire){
														systems_To_override[i].LocalOverrides[m]=1;
													}
													if(Propagator.enable_flyaway & systems_To_override[i].is_butterfly & systems_To_override[i].is_fire){
														systems_To_override[i].LocalOverrides[m]=3;
													}else
													if(Propagator.enable_flyaway & systems_To_override[i].is_butterfly ){
														systems_To_override[i].LocalOverrides[m]=2;
													}
													if(Propagator.enable_LocalWind & systems_To_override[i].is_grass ){
															systems_To_override[i].LocalOverrides[m]=4;
													}
												}else{
														if(this.is_fire & systems_To_override[i].is_ice){
															systems_To_override[i].LocalOverrides[m]=1;
														}
														if(this.is_ice & systems_To_override[i].is_fire){
															systems_To_override[i].LocalOverrides[m]=1;
														}
														if(this.enable_flyaway & systems_To_override[i].is_butterfly & systems_To_override[i].is_fire){
															systems_To_override[i].LocalOverrides[m]=3;
														}else
														if(this.enable_flyaway & systems_To_override[i].is_butterfly ){
															systems_To_override[i].LocalOverrides[m]=2;
														}
														if(this.enable_LocalWind & systems_To_override[i].is_grass ){
															systems_To_override[i].LocalOverrides[m]=4;
														}
												}

											}

										}
									}
								
								}
							}

							//end overrides
						if(propagator_exists){
						if(!Propagator.Erase_mode){

						if(Propagator.Registered_paint_positions!=null ){
							
							if(Propagator.Registered_paint_positions.Count > ( particle_count/2)){
								//do nothing

							}else{
								

												if(collisionEvents[j][k].colliderComponent != null){//if(collisionEvents[j][k].collider !=null){ //v2.1
													if(collisionEvents[j][k].colliderComponent.gameObject !=null){

										if(other.GetComponent<Rigidbody>()){

											//add force
											if(!other.GetComponent<Rigidbody>().isKinematic){


													float Force =Flame_force*300;

												other.GetComponent<Rigidbody>().AddForceAtPosition((other.transform.position - p11Transform.position).normalized*Force,other.transform.position);

											}

										}											
									
										int is_close_to_other_point_on_object=0;
										if(Random.Range(1,Propagator.propagation_chance_factor+1) == Propagator.propagation_chance_factor){
											
											is_close_to_other_point_on_object =1;
											
										}

										//v2.0
										bool enter_on_mat = false;
										int Decal_ID=0;
										if(Decal_per_mat){
											bool Has_any = false;
											int Any_ID=0;
											//search material name of collider renderer for available strings and keep ids found
											for(int k1 = 0;k1< Mat_names.Count;k1++){
																if(collisionEvents[j][k].colliderComponent.gameObject.GetComponent<Renderer>().material.name.Contains(Mat_names[k1])){
													Decal_ID = k1; enter_on_mat = true;
												}
												if(Mat_names[k1] == "Any"){
													Any_ID=k1;Has_any=true;
												}
											}
											if(!enter_on_mat & Has_any){//make a final check for "Any"
												Decal_ID = Any_ID;enter_on_mat = true;
											}
										}else{
											enter_on_mat = true;
										}


										if(is_close_to_other_point_on_object == 1 & enter_on_mat){
								Propagator.Registered_paint_positions.Add(collisionEvents[j][k].intersection);
								Propagator.Registered_paint_rotations.Add(collisionEvents[j][k].normal);
								Propagator.Registered_paint_times.Add (Time.fixedTime);

															//v1.6
															if(!Propagator.gameobject_mode){
																Propagator.Registered_paint_size.Add (Propagator.p11.main.startSize.constant+Propagator.Random_size_factor);
															}

											Propagator.Updated_Registered_paint_positions.Add(collisionEvents[j][k].intersection);
															Propagator.Particle_color.Add (new Vector4(0,0,0,999));

												//GAMEOBJECTS
												#region GAMEOBJECTS
												if(Propagator.gameobject_mode & (Application.isPlaying) ){
													if(Propagator.Gameobj_instances.Count < (Propagator.particle_count)){
														//GameObject TEMP = Instantiate(Propagator.Gameobj,Propagator.Registered_paint_positions[Propagator.Registered_paint_positions.Count-1],Quaternion.identity)as GameObject;
														
														//v2.0 - pooling
														GameObject TEMP = null;
														if(Propagator.Use_pooling & !Decal_per_mat){
															if(Propagator.Item_pool.Count > 0){//if items exist, grab, enable and remove from the list
																
																TEMP = Propagator.Item_pool[0].gameObject;
																TEMP.transform.position = Propagator.Registered_paint_positions[Propagator.Registered_paint_positions.Count-1];
																TEMP.SetActive(true);
																Propagator.Item_pool.RemoveAt(0);
																//Debug.Log ("Done");
															}else{
																TEMP = Instantiate(Propagator.Gameobj,Propagator.Registered_paint_positions[Propagator.Registered_paint_positions.Count-1],Quaternion.identity)as GameObject;
																//Debug.Log ("Zero");
															}
														}else{
																if(Decal_per_mat){
																	//Debug.Log (Decal_ID);
																	TEMP = Instantiate(Decals_for_mat[Decal_ID].gameObject,Propagator.Registered_paint_positions[Propagator.Registered_paint_positions.Count-1],Quaternion.identity)as GameObject;
																}else{
																	TEMP = Instantiate(Propagator.Gameobj,Propagator.Registered_paint_positions[Propagator.Registered_paint_positions.Count-1],Quaternion.identity)as GameObject;
																}
														}


														if(TEMP.GetComponent<Collider>()!=null){
															if(Propagator.Remove_colliders ){
																TEMP.GetComponent<Collider>().enabled=false;
															}else{TEMP.GetComponent<Collider>().enabled=true;}
														}
														
														Propagator.Gameobj_instances.Add(TEMP.transform);
														TEMP.transform.position = Propagator.Registered_paint_positions[Propagator.Registered_paint_positions.Count-1];
														
														if(Propagator.Angled){
															
															TEMP.transform.localEulerAngles = Propagator.Registered_paint_rotations[Propagator.Registered_paint_positions.Count-1];
															
														}
														
														TEMP.transform.parent = Propagator.Parent_OBJ.transform;

														//v2.0
														//Debug.DrawRay(	TEMP.transform.position,collisionEvents[j][k].normal,Color.blue,2);
														
														if(propagator_exists){
															if(Propagator.normal_On_Y){
																TEMP.transform.rotation = Quaternion.FromToRotation(Vector3.up,collisionEvents[j][k].normal);
															}else{
																TEMP.transform.LookAt(TEMP.transform.position + collisionEvents[j][k].normal, Vector3.right);
															}
														}else{
															if(normal_On_Y){
																TEMP.transform.rotation = Quaternion.FromToRotation(Vector3.up,collisionEvents[j][k].normal);
															}else{
																TEMP.transform.LookAt(TEMP.transform.position + collisionEvents[j][k].normal, Vector3.right);
															}
														}

														//v1.2.2
														Propagator.Updated_gameobject_positions.Add (TEMP.transform.position);

																	//v1.6
																	Propagator.Registered_paint_size.Add (Propagator.Gameobj_instances[Propagator.Gameobj_instances.Count-1].localScale.x);
													}													
													else{//v1.6
																	Propagator.Registered_paint_size.Add (Propagator.p11.main.startSize.constant+Propagator.Random_size_factor);
													}
												}
												#endregion
												//END GAMEOBJECTS

												Propagator.LocalOverrides.Add(0);
												Propagator.PaintTypes.Add(ParticlePropagationPDM.PaintType.ParticleCollision);

																			
															Propagator.Emitter_objects.Add(collisionEvents[j][k].colliderComponent.gameObject.transform);
															Propagator.Registered_initial_positions.Add(collisionEvents[j][k].colliderComponent.gameObject.transform.position);
															Propagator.Registered_initial_scale.Add (collisionEvents[j][k].colliderComponent.gameObject.transform.localScale);

															Propagator.Registered_initial_rotation.Add(collisionEvents[j][k].colliderComponent.gameObject.transform.rotation);

															if(Add_effects.Count > 0){
																if(Add_effects[Decal_ID] !=null){
																	Add_effects[Decal_ID].Registered_paint_positions.Add(collisionEvents[j][k].intersection);
																	Add_effects[Decal_ID].Registered_paint_rotations.Add(collisionEvents[j][k].normal);
																	Add_effects[Decal_ID].Registered_paint_times.Add (Time.fixedTime);
																	
																	//v1.6
																	if(!Add_effects[Decal_ID].gameobject_mode){
																		Add_effects[Decal_ID].Registered_paint_size.Add (Add_effects[Decal_ID].p11.main.startSize.constant+Add_effects[Decal_ID].Random_size_factor);
																	}
																	
																	Add_effects[Decal_ID].Updated_Registered_paint_positions.Add(collisionEvents[j][k].intersection);
																	Add_effects[Decal_ID].Particle_color.Add (new Vector4(0,0,0,999));

																	Add_effects[Decal_ID].LocalOverrides.Add(0);
																	Add_effects[Decal_ID].PaintTypes.Add(ParticlePropagationPDM.PaintType.ParticleCollision);
																	
																	
																	Add_effects[Decal_ID].Emitter_objects.Add(collisionEvents[j][k].colliderComponent.gameObject.transform);
																	Add_effects[Decal_ID].Registered_initial_positions.Add(collisionEvents[j][k].colliderComponent.gameObject.transform.position);
																	Add_effects[Decal_ID].Registered_initial_scale.Add (collisionEvents[j][k].colliderComponent.gameObject.transform.localScale);
																	
																	Add_effects[Decal_ID].Registered_initial_rotation.Add(collisionEvents[j][k].colliderComponent.gameObject.transform.rotation);
																}
															}

										}//end if close check
									}
								}								
							}
						}
					}else{
								for (int i=0;i< Propagator.Updated_Registered_paint_positions.Count;i++){
									
									if( Vector3.Distance(collisionEvents[j][k].intersection,Propagator.Updated_Registered_paint_positions[i]) < (0.5f*Propagator.brush_size))
									{
										
										//GAMEOBJECTS
										#region GAMEOBJECTS
										if(Propagator.gameobject_mode & (Application.isPlaying) ){

											Destroy(Propagator.Gameobj_instances[i].gameObject);
											Propagator.Gameobj_instances.RemoveAt(i);
											Propagator.Updated_gameobject_positions.RemoveAt(i);
										}
										#endregion
										//END GAMEOBJECTS

										Propagator.LocalOverrides.RemoveAt(i);
										Propagator.PaintTypes.RemoveAt(i);
										
										Propagator.Emitter_objects.RemoveAt(i);
										
										Propagator.Updated_Registered_paint_positions.RemoveAt(i);
												Propagator.Particle_color.RemoveAt(i);

										Propagator.Registered_paint_rotations.RemoveAt(i);
										Propagator.Registered_paint_times.RemoveAt(i);
										Propagator.Registered_paint_size.RemoveAt(i);
										
										Propagator.Registered_paint_positions.RemoveAt(i);
										Propagator.Registered_initial_positions.RemoveAt(i);
										Propagator.Registered_initial_rotation.RemoveAt(i);
										Propagator.Registered_initial_scale.RemoveAt(i);
										
										break;
									}									
								}
					}
					}					
						
					}
				}
				
				}//END fire/ice layers check
			}//END layers check
		}//END if propagator use collisions check
			
	
	}//END oncollision	 

	}}