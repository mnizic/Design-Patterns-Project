namespace mnizic_zadaca_3.Visitor
{
    public class ConcreteComponentVezoviPU : Visitable
    {
        private int ukupanZbroj = 0;
        
        public void inkrementirajZbroj()
        {
            ukupanZbroj++;
        }

        public int dohvatiZbroj()
        {
            return ukupanZbroj;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}