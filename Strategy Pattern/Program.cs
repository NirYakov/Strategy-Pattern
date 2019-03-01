using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Strategy_Pattern
{
    public class Program
    {
        public static void Main()
        {
            Active();
            Console.ReadKey();
        }

        public static void Active()
        {
            // here to do
            List<Customer> customers = new List<Customer>();
            Customer customerFixed = new Customer(new FixedPrice(),i_Name: "Fixed" ,i_Age: 25);
            Customer customerNormal = new Customer(new NormalPrice(),i_Name: "Normal");
            Customer customerHappyHour = new Customer(new HappyHourPrice(), 22 , "Happy");

            customers.Add(customerFixed);
            customers.Add(customerNormal);
            customers.Add(customerHappyHour);

            foreach (Customer customer in customers)
            {
                customer.AddDrinks(4.2f, 2);
                customer.AddDrinks(0.9f, 3);
                customer.AddDrinks(5.5f, 2);
                customer.AddDrinks(1.0f, 1);
            }

            foreach (Customer customer in customers)
            {
                Console.WriteLine(customer);
                customer.PrintTotalBill();
            }

            string breakLinesFormat = @"
 === order by {0} ===
            ";

            // string breakLinesFormat = string.Format ("{0} === order by {1} ==={0}",Environment.NewLine, "{1}");

            Console.WriteLine(breakLinesFormat , "age");

            customers.Sort(new CustomerAgeComparer());

            foreach (Customer customer in customers)
            {
                Console.WriteLine(customer);
                customer.PrintTotalBill();
            }

            Console.WriteLine(breakLinesFormat , "name");

            customers.Sort(new CustomerNameComparer());

            foreach (Customer customer in customers)
            {
                Console.WriteLine(customer);
                customer.PrintTotalBill();
            }
        }
    }

    public class Customer
    {
        private string m_Name;
        private int m_Age;

        public readonly ICollection<float> r_Drinks;
        public IBillingStrategy m_Strategy;

        public Customer(IBillingStrategy i_Strategy, int i_Age = 18, string i_Name = "Unknown")
        {
            r_Drinks = new LinkedList<float>();
            m_Strategy = i_Strategy;
            m_Age = i_Age;
            m_Name = i_Name;
        }

        public int Age
        {
            get { return m_Age; }
            set { m_Age = value; }
        }

        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        public void AddDrinks(float i_Price, int i_Quantity)
        {
            r_Drinks.Add(m_Strategy.GetPrice( i_Price * i_Quantity));
        }

        public void PrintTotalBill()
        {
            float totalSum = 0;
            foreach (float price in r_Drinks)
            {
                totalSum += price;
            }

            Console.WriteLine("Total bill is {0,5:0.00}$ ", totalSum);
        }

        public override string ToString()
        {
            return string.Format("Name: {0} , Age: {1}" , m_Name , m_Age) ;
        }
    }

    public interface IBillingStrategy
    {
        float GetPrice(float i_RawPrice);
    }

    public class NormalPrice : IBillingStrategy
    {
        public float GetPrice(float i_RawPrice)
        {
            return i_RawPrice;
        }
    }

    public class HappyHourPrice : IBillingStrategy
    {
        public float GetPrice(float i_RawPrice)
        {
            return i_RawPrice * 0.5f;
        }
    }

    public class FixedPrice : IBillingStrategy
    {
        private bool m_HasPaidInit = false;
        private const float k_FixedPrice = 20f;

        public float GetPrice(float i_RawPrice)
        {
            float sum = 0;
            if (!m_HasPaidInit)
            {
                m_HasPaidInit = true;
                sum = k_FixedPrice;
            }

            return sum;
        }
    }

    public class CustomerAgeComparer : IComparer<Customer>
    {
        public int Compare(Customer x, Customer y)
        {
           return x.Age - y.Age;
        }
    }

    public class CustomerNameComparer : IComparer<Customer>
    {
        public int Compare(Customer x, Customer y)
        {
            return string.Compare(x.Name , y.Name);
        }
    }
}
