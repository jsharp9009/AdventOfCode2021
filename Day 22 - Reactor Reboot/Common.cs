namespace ReactorReboot{
     public class Instruction{
        public bool On {get; set;}
        public Cube Cube {get;set;}
    }

    public class Cube{
        public Cube(){}

        public Cube(int xmin, int xmax, int ymin, int ymax, int zmin, int zmax){
            xMin = xmin;
            xMax = xmax;
            yMin = ymin;
            yMax = ymax;
            zMin = zmin;
            zMax = zmax;
        }

        public int xMin {get;set;}
        public int yMin {get;set;}
        public int zMin {get;set;}
        public int xMax {get;set;}
        public int yMax{get;set;}
        public int zMax{get;set;}
    }
}