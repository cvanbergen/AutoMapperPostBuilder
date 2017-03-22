namespace AutoMapperPostBuilder_Sample
{
    class SampleClassA
    {
        public string Prop1 { get; set; }
        public int Prop2 { get; set; }
        public double Prop3 { get; set; }
        public decimal Prop4 { get; set; }
        public decimal Prop6 { get; set; }

        public SampleClassA_1 Prop5 { get; set; }

    }

    class SampleClassA_1
    {
        public string InnerProp1 { get; set; }

        public int InnerProp2 { get; set; }

    }

}
