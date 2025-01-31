using UnityEngine;

namespace Bulb
{
    class ModuleLightBulb : ModuleLight
    {
        [KSPField(guiActive = true, guiName = "#autoLOC_6001402", isPersistant = true)]
        [UI_FloatRange(maxValue = 1, minValue = 0, scene = UI_Scene.Flight, stepIncrement = 0.01f)]
        protected float red = 0;

        [KSPField(guiActive = true, guiName = "#autoLOC_6001403", isPersistant = true)]
        [UI_FloatRange(maxValue = 1, minValue = 0, scene = UI_Scene.Flight, stepIncrement = 0.01f)]
        protected float green = 0;

        [KSPField(guiActive = true, guiName = "#autoLOC_6001404", isPersistant = true)]
        [UI_FloatRange(maxValue = 1, minValue = 0, scene = UI_Scene.Flight, stepIncrement = 0.01f)]
        protected float blue = 0;

        [KSPField(guiActive = false, isPersistant = true)]
        bool bulbColorRecorded = false;

        Light light;
        Renderer emissive;

        protected bool ready = false;

        public override void OnStart(StartState state)
        {
            base.OnStart(state);
            if ((state != StartState.None) && (state != StartState.Editor) && (part != null))
            {
                light = part.FindModelComponent<Light>();
                emissive = part.FindModelComponent<Renderer>();
                if ((light != null) && (emissive != null))
                {
                    ready = true;
                }
            }
        }

        public override void OnUpdate()
        {
            if (ready)
            {
                if (HighLogic.LoadedSceneIsFlight)
                {
                    if (!bulbColorRecorded)
                    {
                        recordLightColor();
                    }
                    else
                    {
                        setLightColor();
                    }
                }
            }
        }

        public void recordLightColor()
        {
            if (light == null) return;
            red = light.color.r;
            green = light.color.g;
            blue = light.color.b;
            bulbColorRecorded = true;
        }

        public void setLightColor()
        {
            if (emissive == null || light == null) return;
            if (light.color.r != red || light.color.g != green || light.color.b != blue)
            {
                light.color = new Color(red, green, blue, 1);
                emissive.material.SetColor("_EmissiveColor", light.color);
            }
        }
    }
}
