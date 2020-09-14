using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using api_insEXchange.Module;


namespace api_insEXchange
{
    class Program
    {
        static dbtestEntities rtr = new dbtestEntities();
        

        static void Main(string[] args)
        {
            JObject obj = APIGET.API_GETJOBJECT("https://api.exchangeratesapi.io/history?start_at=2019-01-01&end_at=2020-07-31&base=USD");

            //var json = JsonConvert.SerializeObject(obj["rates"]);
            //var dictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            int i = 0;
            int r = 1;
            foreach ( var pair in JObject.Parse( obj["rates"].ToString() ) )
            {
                
                DateTime d = Convert.ToDateTime(pair.Key.ToString());
                var cdt = rtr.EXCHANGERATES.Where( w => w.EXC_DATE ==  d ).ToList();
                
                if ( cdt.Count() < 1 )
                {
                    //EXCHANGERATES exc = new EXCHANGERATES();
                    //exc.EXC_DATE = d;
                    EXCHANGERATES result = JsonConvert.DeserializeObject<EXCHANGERATES>( pair.Value.ToString() );
                    result.EXC_DATE = d;
                    rtr.EXCHANGERATES.Add(result);
                    //foreach (var dt in JObject.Parse( pair.Value.ToString() ))
                    //{
                    //    
                    if( i % 5 == 0 )
                    {
                        
                        r = r + 1;
                        Console.WriteLine("■".PadRight(r, '■') + " " + d.ToString("yyyy-MM-dd"));
                        Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop - 1);
                    }
                    i++;
                    //}

                }
                else
                {
                    Console.Write("No data");
                }

            }
            rtr.SaveChanges();
            //Console.WriteLine(obj["rates"]);

            //Console.SetCursorPosition(Con, Console.CursorTop); 
            //Console.WriteLine("Complete !");
            Console.ReadKey();
        }
    }
}
