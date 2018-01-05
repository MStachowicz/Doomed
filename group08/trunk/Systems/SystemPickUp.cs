using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenGL_Game.Components;
using OpenGL_Game.Objects;
using System.IO;
using static OpenGL_Game.Components.ComponentAI;


namespace OpenGL_Game.Systems
{
    class SystemPickUp : ISystem
    {
        const ComponentTypes MASK = (ComponentTypes.COMPONENT_HEALTH | ComponentTypes.COMPONENT_AMMO | ComponentTypes.COMPONENT_AI | ComponentTypes.COMPONENT_POSITION | ComponentTypes.COMPONENT_PICK_UP);

        
        public string Name
        {
            get { return "SystemPickUp"; }
        }

        public void OnAction(Entity entity)
        {
            if (entity.Name == "Health" && entity.Name== "Ammo" && entity.Name== "Drone_Dea")
            {
                
            }
            if ((entity.Mask & MASK) == MASK)
            {
                List<IComponent> components = entity.Components;

                IComponent healthComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_HEALTH;
                });

                IComponent ammoComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AMMO;
                });

                ComponentAI aiComponent = (ComponentAI)components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_AI;
                });
                AIStates state = aiComponent.CurrentState;

                IComponent positionComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_POSITION;
                });

                IComponent pickupComponent = components.Find(delegate (IComponent component)
                {
                    return component.ComponentType == ComponentTypes.COMPONENT_PICK_UP;
                });

                PowerUp((ComponentHealth)healthComponent, (ComponentAmmo)ammoComponent,(ComponentPickUp)pickupComponent);
                
            }
            
        }
       
      
        private void PowerUp(ComponentHealth h, ComponentAmmo a, ComponentPickUp up)
        {
            h.Health += up.Pick_health ;
            a.Ammo += up.Pick_ammo ;   
        }

       public void Delete(ComponentPickUp pp, ComponentPosition pos ,Entity ent)
        {
            //bool PL = ent.Name == "Player";
            //bool HE = ent.Name == "Helath";
           // bool AM = ent.Name == "Ammo";
           //bool DD = ent.Name == "Drone_Dea";

            //if ()
            {
                
            }
        }



    }
}







