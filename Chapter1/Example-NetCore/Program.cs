using System;

namespace Example_NetCore
{


    public class MyClass {
        public string MyProperty {get; set;}
        public MyClass() { }
    }


    struct Greeting {
        public string Value { get; set; }
    }

    class Name {
        public string Value { get; set; }
    }




    
    class Program
    {
        /////
        static void Process(ref Greeting greet, ref Name name) {
            greet.Value = "Goodbye";
            name.Value = "You";
        }

        ////////
        private readonly object _myLock = new object();

        public void Foo() {
            lock(_myLock){
                Bar();
            }
        }

        private void Bar(){
            lock(_myLock){
                Console.WriteLine("success");
            }
        }

        static void Main(string[] args)
        {
            
            /*
                Task 1
             */
            MyClass myObject = new MyClass();

            myObject.MyProperty = "Value"; // null 
            // myObject = null;

            String myValue;

            /*
            if (myObject != null) {
                if (myObject.MyProperty != null) {
                    myValue = myObject.MyProperty;
                }
                else{
                    myValue = "Not available";
                }
            }
            else {
                myValue = "Not available";
            }
            */

            myValue = (myObject!=null)? (myObject.MyProperty!=null)? myObject.MyProperty : "Not available" : "Not available" ;
            // alternative
            // myValue = myObject?.MyProperty?.MyProperty;

            Console.WriteLine(myValue);

            /*
                Task 2
             */
            Greeting greet = new Greeting { Value = "Hello" };
            Name name = new Name { Value = "World" };

            // Process(greet, name); // "Hello You"
            Process(ref greet, ref name); // "Goodbye You"

            Console.WriteLine(greet.Value + " " + name.Value);

            /*
                Task 3
             */
            Program p = new Program();
            p.Foo();
        }
    }
}
