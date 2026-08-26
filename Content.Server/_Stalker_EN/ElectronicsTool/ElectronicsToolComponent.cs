namespace Content.Server.ElectronicsTool.Components
{
    [RegisterComponent]
    public sealed partial class ElectronicsToolComponent : Component
    {
        [DataField]
        public float SearchTime = 5;

        [DataField]
        public float Probability = 0.5f;

        [DataField]
        public string Loot = "RandomElectronicsToolSpawner";
        
        // ST:OW begin - multi-roll electronics salvage
        [DataField]
        public int RollsMin = 1;

        [DataField]
        public int RollsMax = 1;

        [DataField]
        public int RollsHardCap = 6;
        // ST:OW end
    }
}
