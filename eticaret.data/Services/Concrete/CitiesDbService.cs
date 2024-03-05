using eticaret.data.Services.Abstract;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eticaret.data.Services.Concrete
{
    public class CitiesDbService: ICitiesDbService
    {
        private readonly IConfiguration _configuration;

        public CitiesDbService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Tuple<int, string>> AllCities
        {
            get
            {
                List<Tuple<int, string>> result = new();
                string con_string = _configuration["ConnectionStrings:Cities"];
                MySqlConnection connection = new MySqlConnection(con_string);
                connection.Open();
                var command = new MySqlCommand("select * from iller;", connection);
                using var reader = command.ExecuteReader();
                while (reader.Read())
                    result.Add(new Tuple<int, string>(reader.GetInt32(0), reader.GetString(1)));
                connection.Close();
                return result;
            }
        }
        public List<Tuple<int, string>> GetDistrictByCityId(int id)
        {
            List<Tuple<int, string>> result = new();
            string con_string = _configuration["ConnectionStrings:Cities"];
            MySqlConnection connection = new MySqlConnection(con_string);
            connection.Open();
            var command = new MySqlCommand($"select * from ilceler where il_id={id}", connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
                result.Add(new Tuple<int, string>(reader.GetInt32(0), reader.GetString(2)));
            connection.Close();
            return result;
        }
        public string GetCityById(int id)
            => GetOneByQuery($"select * from iller where id={id}", 1);
        

        public string GetDistrictById(int id)
            => GetOneByQuery($"select * from ilceler where id={id}", 2);

        public string GetNeighborhoodById(int id)
            => GetOneByQuery($"select * from mahalleler where id={id}", 2);

        public List<Tuple<int, string>> GetNeighborhoodsByDistrictId(int id)
        {
            List<Tuple<int, string>> result = new();
            string con_string = _configuration["ConnectionStrings:Cities"];
            MySqlConnection connection = new MySqlConnection(con_string);
            connection.Open();
            var command = new MySqlCommand($"select * from mahalleler m left join semtler s on s.id=m.semt_id where s.ilce_id={id}", connection);
            using var reader = command.ExecuteReader();
            while (reader.Read())
                result.Add(new Tuple<int, string>(reader.GetInt32(0), reader.GetString(2)));
            connection.Close();
            return result;
        }

        public bool IsValid(string city, string district, string neighborhood)
        {
            MySqlConnection connection;
            MySqlCommand       command;
            string        con_string;
            int     id=-1, counter=0;

            con_string = _configuration["ConnectionStrings:Cities"];
            connection = new MySqlConnection(con_string);
            connection.Open();


            command = new MySqlCommand($"select * from iller where il_adi='{city}'", connection);
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                    counter++;
                }
                if (counter == 0) { return false; };
            }


            command = new MySqlCommand($"select * from ilceler where il_id={id} and ilce_adi='{district}'", connection);
            counter = 0;
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                    counter++;
                }
                if (counter == 0) { return false; };
            }


            //command = new SqlCommand($"select * from dbo.SemtMah where ilceId='{id}' and MahalleAdi='{neighborhood}'", connection);
            //counter = 0;

            return true;
        }

        //private bool CountExecutes(SqlCommand command)
        //{
        //    int counter = 0;
        //    using (var reader = command.ExecuteReader())
        //    {
        //        while (reader.Read())
        //        {
        //            counter++;
        //        }
        //        if (counter == 0) { return false; };
        //    }
        //    return true;
        //}

        private string GetOneByQuery(string query, int index)
        {
            string result = "", con_string;
            MySqlConnection connection;
            MySqlCommand command;
            con_string = _configuration["ConnectionStrings:Cities"];
            connection = new MySqlConnection(con_string);
            connection.Open();
            command = new MySqlCommand(query, connection);

            using var reader = command.ExecuteReader();
            while (reader.Read())
                result = reader.GetString(index);
            reader.Close();
            return result;
        }
    }
}
