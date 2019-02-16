namespace SweepAndPrune
{
    public class Item
    {
        public int X1 { get; set; }
        public int Y1 { get; set; }
        public int X2
        {
            get
            {
                return this.X1 + 10;
            }
            set
            {
            }
        }
        public int Y2
        {
            get
            {
                return this.Y1 + 10;
            }
            set
            {
            }
        }
        public string Name { get; set; }
        public Item(int x1, int y1, string name)
        {
            this.X1 = x1;
            this.Y1 = y1;
            this.Name = name;
        }

        public bool Intersect(Item other)
        {
            return this.X1 <= other.X2 &&
                   this.X2 >= other.X1 &&
                   this.Y1 <= other.Y2 &&
                   this.Y2 >= other.Y1;
        }
    }
}
