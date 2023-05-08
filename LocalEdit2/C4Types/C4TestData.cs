namespace LocalEdit2.C4Types
{
    public class C4TestData
    {
        // READ BEFORE GRIPING:
        //
        // Yes, I know it is not "best practice" to have a getter that creates something!
        // By using a getter, instead of a method, the places where thie is used "look better"

        public static C4Item InternalPerson
        {
            get
            {
                C4Item rtnVal = new();
                rtnVal.Alias = "INT_PERS";
                rtnVal.Description = "A person who works inside the organization";
                //rtnVal.From = string.Empty;
                //rtnVal.IsDatabase = false;
                rtnVal.IsExternal = false;
                rtnVal.ItemType = C4TypeEnum.Person;
                //rtnVal.Technology = String.Empty;
                rtnVal.Text = "Internal Person";
                //rtnVal.To = String.Empty;
                return rtnVal;
            }
        }

        public static C4Item ExternalPerson
        {
            get
            {
                C4Item rtnVal = new();
                rtnVal.Alias = "EXT_PERS";
                rtnVal.Description = "A person who works outside the organization";
                //rtnVal.From = string.Empty;
                //rtnVal.IsDatabase = false;
                rtnVal.IsExternal = true;
                rtnVal.ItemType = C4TypeEnum.Person;
                //rtnVal.Technology = string.Empty;
                rtnVal.Text = "External Person";
                //rtnVal.To = String.Empty;
                return rtnVal;
            }
        }

        public static C4Item EnterpriseBoundary
        {
            get
            {
                C4Item rtnVal = new();
                rtnVal.Alias = "ENT_BNDRY";
                rtnVal.Description = "Boundary around an Enterprise";
                //rtnVal.From = string.Empty;
                //rtnVal.IsDatabase = false;
                //rtnVal.IsExternal = true;
                rtnVal.ItemType = C4TypeEnum.EnterpriseBoundary;
                //rtnVal.Technology = string.Empty;
                rtnVal.Text = "Enterprise A";
                //rtnVal.To = String.Empty;
                return rtnVal;
            }
        }
        public static C4Item ContainerBoundary
        {
            get
            {
                C4Item rtnVal = new();
                rtnVal.Alias = "CONT_BNDRY";
                rtnVal.Description = "Boundary around a Container";
                //rtnVal.From = string.Empty;
                //rtnVal.IsDatabase = false;
                //rtnVal.IsExternal = true;
                rtnVal.ItemType = C4TypeEnum.ContainerBoundary;
                //rtnVal.Technology = string.Empty;
                rtnVal.Text = "Container A";
                //rtnVal.To = String.Empty;
                return rtnVal;
            }
        }
        public static C4Item SystemBoundary
        {
            get
            {
                C4Item rtnVal = new();
                rtnVal.Alias = "SYST_BNDRY";
                rtnVal.Description = "Boundary around a System";
                //rtnVal.From = string.Empty;
                //rtnVal.IsDatabase = false;
                //rtnVal.IsExternal = true;
                rtnVal.ItemType = C4TypeEnum.SystemBoundary;
                //rtnVal.Technology = string.Empty;
                rtnVal.Text = "System A";
                //rtnVal.To = String.Empty;
                return rtnVal;
            }
        }
        public static C4Item Boundary
        {
            get
            {
                C4Item rtnVal = new();
                rtnVal.Alias = "BNDRY";
                rtnVal.Description = "Ad-hoc Boundary";
                //rtnVal.From = string.Empty;
                //rtnVal.IsDatabase = false;
                //rtnVal.IsExternal = true;
                rtnVal.ItemType = C4TypeEnum.Boundary;
                //rtnVal.Technology = string.Empty;
                rtnVal.Text = "Ad-hoc Boundary";
                //rtnVal.To = String.Empty;
                return rtnVal;
            }
        }
        public static C4Item Component
        {
            get
            {
                C4Item rtnVal = new();
                rtnVal.Alias = "COMP";
                rtnVal.Description = "A Component";
                //rtnVal.From = string.Empty;
                rtnVal.IsDatabase = false;
                //rtnVal.IsExternal = true;
                rtnVal.ItemType = C4TypeEnum.Component;
                rtnVal.Technology = "Technology";
                rtnVal.Text = "Component 1";
                //rtnVal.To = String.Empty;
                return rtnVal;
            }
        }
        public static C4Item Database
        {
            get
            {
                C4Item rtnVal = new();
                rtnVal.Alias = "DB_1";
                rtnVal.Description = "A Database";
                //rtnVal.From = string.Empty;
                rtnVal.IsDatabase = true;
                //rtnVal.IsExternal = true;
                rtnVal.ItemType = C4TypeEnum.Database;
                rtnVal.Technology = "No SQL";
                rtnVal.Text = "Database 1";
                //rtnVal.To = String.Empty;
                return rtnVal;
            }
        }
        public static C4Item Container
        {
            get
            {
                C4Item rtnVal = new();
                rtnVal.Alias = "CONT_1";
                rtnVal.Description = "A Container";
                //rtnVal.From = string.Empty;
                //rtnVal.IsDatabase = false;
                //rtnVal.IsExternal = true;
                rtnVal.ItemType = C4TypeEnum.Container;
                rtnVal.Technology = "Unknown";
                rtnVal.Text = "Container 1";
                //rtnVal.To = String.Empty;
                return rtnVal;
            }
        }
        public static C4Item Node
        {
            get
            {
                C4Item rtnVal = new();
                rtnVal.Alias = "NODE_1";
                rtnVal.Description = "A Node";
                //rtnVal.From = string.Empty;
                //rtnVal.IsDatabase = false;
                //rtnVal.IsExternal = true;
                rtnVal.ItemType = C4TypeEnum.Node;
                rtnVal.Technology = "Unknown";
                rtnVal.Text = "Node 1";
                //rtnVal.To = String.Empty;
                return rtnVal;
            }
        }
        public static C4Item InternalSystem
        {
            get
            {
                C4Item rtnVal = new();
                rtnVal.Alias = "SYSTEM_1";
                rtnVal.Description = "A System (Internal)";
                //rtnVal.From = string.Empty;
                rtnVal.IsDatabase = false;
                rtnVal.IsExternal = false;
                rtnVal.ItemType = C4TypeEnum.System;
                rtnVal.Technology = "Unknown";
                rtnVal.Text = "System 1 (Internal)";
                //rtnVal.To = String.Empty;
                return rtnVal;
            }
        }
        public static C4Item ExternalSystem
        {
            get
            {
                C4Item rtnVal = new();
                rtnVal.Alias = "SYSTEM_1";
                rtnVal.Description = "A System (External)";
                //rtnVal.From = string.Empty;
                rtnVal.IsDatabase = false;
                rtnVal.IsExternal = true;
                rtnVal.ItemType = C4TypeEnum.System;
                rtnVal.Technology = "Unknown";
                rtnVal.Text = "System 2 (External)";
                //rtnVal.To = String.Empty;
                return rtnVal;
            }
        }
        public static C4Item InternalDatabaseSystem
        {
            get
            {
                C4Item rtnVal = new();
                rtnVal.Alias = "SYSTEM_1";
                rtnVal.Description = "A System (Internal Database)";
                //rtnVal.From = string.Empty;
                rtnVal.IsDatabase = true;
                rtnVal.IsExternal = false;
                rtnVal.ItemType = C4TypeEnum.System;
                rtnVal.Technology = "Unknown";
                rtnVal.Text = "System 3 (Internal Database)";
                //rtnVal.To = String.Empty;
                return rtnVal;
            }
        }
        public static C4Item ExternalDatabaseSystem
        {
            get
            {
                C4Item rtnVal = new();
                rtnVal.Alias = "SYSTEM_1";
                rtnVal.Description = "A System (External Database)";
                //rtnVal.From = string.Empty;
                rtnVal.IsDatabase = true;
                rtnVal.IsExternal = true;
                rtnVal.ItemType = C4TypeEnum.System;
                rtnVal.Technology = "Unknown";
                rtnVal.Text = "System 4 (External Database)";
                //rtnVal.To = String.Empty;
                return rtnVal;
            }
        }
    }
}
