using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using TCCVetCare.Models;
using MySql.Data.MySqlClient;

namespace TCCVetCare.Repositories
{
    public class CustomerRepository
    {
        DatabaseConnection con = new DatabaseConnection();
        int idAddress = 0;
        public void insertCustomer(CustomerModel customer)
        {
            MySqlCommand cmd = new MySqlCommand(
                "insert into tbCustomer values(default, @nameCustomer,@cpfCustomer,  @phoneCustomer,  @emailCustomer, @passwordCustomer)",
                con.ConectarBD()
            );
            cmd.Parameters.Add("@nameCustomer", MySqlDbType.VarChar).Value = customer.nameCustomer;
            cmd.Parameters.Add("@cpfCustomer", MySqlDbType.VarChar).Value = customer.cpfCustomer;
            cmd.Parameters.Add("@emailCustomer", MySqlDbType.VarChar).Value =
                customer.emailCustomer;
            cmd.Parameters.Add("@passwordCustomer", MySqlDbType.VarChar).Value =
                customer.passwordCustomer;
            cmd.Parameters.Add("@phoneCustomer", MySqlDbType.VarChar).Value =
                customer.phoneCustomer;

            cmd.ExecuteNonQuery();
            con.DesconectarBD();
        }

        public List<CustomerModel> GetCustomer()
        {
            List<CustomerModel> listCustomer = new List<CustomerModel>();
            MySqlCommand cmd = new MySqlCommand("select * from tbCustomer", con.ConectarBD());

            //adapter para lista
            MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);

            //tabela virtual
            DataTable db = new DataTable();
            adapter.Fill(db);

            con.DesconectarBD();

            //enquanto existir linhas(registros) no banco
            //o foreach irá adicionar os valors vindo do banco nos atributos da ModelCliente
            foreach (DataRow dr in db.Rows)
            {
                listCustomer.Add(
                    new CustomerModel
                    {
                        idCustomer = Convert.ToString(dr["idCustomer"]),
                        nameCustomer = Convert.ToString(dr["nameCustomer"]),
                        cpfCustomer = Convert.ToString(dr["cpfCustomer"]),
                        phoneCustomer = Convert.ToString(dr["phoneCustomer"]),
                        emailCustomer = Convert.ToString(dr["emailCustomer"]),
                        passwordCustomer = Convert.ToString(dr["passwordCustomer"]),

                    }
                );
            }
            return listCustomer;
        }

        public List<CustomerModel> GetCustomersWithAddress()
        {
            List<CustomerModel> customers = new List<CustomerModel>();

            string connectionString = "Server=localhost;DataBase=teste2;User=root;pwd=12345678";

            using (MySqlConnection cn = new MySqlConnection(connectionString))
            {
                cn.Open();

                string query = @"SELECT tbCustomer.idCustomer, tbCustomer.nameCustomer, tbCustomer.cpfCustomer,
                               tbCustomer.phoneCustomer, tbCustomer.emailCustomer, tbCustomer.passwordCustomer,
                               tbAddressCustomer.idAddress, tbAddressCustomer.zipCode, tbAddressCustomer.streetName,
                               tbAddressCustomer.streetNumber, tbAddressCustomer.addressComplement,
                               tbAddressCustomer.neighborhood, tbAddressCustomer.city, tbAddressCustomer.state
                                FROM tbCustomer
                                INNER JOIN tbAddressCustomer ON tbCustomer.idAddress = tbAddressCustomer.idAddress"
                ;
                MySqlCommand cmd = new MySqlCommand(query, cn);


                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CustomerModel customer = new CustomerModel();
                        customer.idCustomer = reader.GetString("idCustomer");
                        customer.idAddress = reader.GetString("idAddress");
                        customer.nameCustomer = reader.GetString("nameCustomer");
                        customer.cpfCustomer = reader.GetString("cpfCustomer");
                        customer.phoneCustomer = reader.GetString("phoneCustomer");
                        customer.emailCustomer = reader.GetString("emailCustomer");
                        customer.passwordCustomer = reader.GetString("passwordCustomer");
                        customer.zipCode = reader.GetString("zipCode");
                        customer.streetName = reader.GetString("streetName");
                        customer.streetNumber = reader.GetString("streetNumber");
                        customer.addressComplement = reader.GetString("addressComplement");
                        customer.neighborhood = reader.GetString("neighborhood");
                        customer.city = reader.GetString("city");
                        customer.state = reader.GetString("state");

                        customers.Add(customer);
                    }
                }

                cn.Close();
            }

            return customers;
        }

        public List<CustomerModel> GetCustomersWithAddress(String idCustomer)
        {
            List<CustomerModel> customers = new List<CustomerModel>();

            string connectionString = "Server=localhost;DataBase=teste2;User=root;pwd=12345678";

            using (MySqlConnection cn = new MySqlConnection(connectionString))
            {
                cn.Open();

                string query = @"SELECT tbCustomer.idCustomer, tbCustomer.nameCustomer, tbCustomer.cpfCustomer,
                               tbCustomer.phoneCustomer, tbCustomer.emailCustomer, tbCustomer.passwordCustomer,
                               tbAddressCustomer.idAddress, tbAddressCustomer.zipCode, tbAddressCustomer.streetName,
                               tbAddressCustomer.streetNumber, tbAddressCustomer.addressComplement,
                               tbAddressCustomer.neighborhood, tbAddressCustomer.city, tbAddressCustomer.state
                                FROM tbCustomer
                                INNER JOIN tbAddressCustomer ON tbCustomer.idAddress = tbAddressCustomer.idAddress
                                WHERE tbCustomer.idCustomer = @idCustomer;"
                ;

                MySqlCommand cmd = new MySqlCommand(query, cn);
                cmd.Parameters.AddWithValue("@idCustomer", idCustomer);

                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        CustomerModel customer = new CustomerModel();
                        customer.idCustomer = reader.GetString("idCustomer");
                        customer.idAddress = reader.GetString("idAddress");
                        customer.nameCustomer = reader.GetString("nameCustomer");
                        customer.cpfCustomer = reader.GetString("cpfCustomer");
                        customer.phoneCustomer = reader.GetString("phoneCustomer");
                        customer.emailCustomer = reader.GetString("emailCustomer");
                        customer.passwordCustomer = reader.GetString("passwordCustomer");
                        customer.zipCode = reader.GetString("zipCode");
                        customer.streetName = reader.GetString("streetName");
                        customer.streetNumber = reader.GetString("streetNumber");
                        customer.addressComplement = reader.GetString("addressComplement");
                        customer.neighborhood = reader.GetString("neighborhood");
                        customer.city = reader.GetString("city");
                        customer.state = reader.GetString("state");

                        customers.Add(customer);
                    }
                }

                cn.Close();
            }

            return customers;
        }


        public bool DeleteCustomer(int id)
        {
            MySqlCommand cmd = new MySqlCommand(
                "delete from tbCustomer where idCustomer=@id",
                con.ConectarBD()
            );
            cmd.Parameters.AddWithValue("id", id);

            int i = cmd.ExecuteNonQuery();
            con.DesconectarBD();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateCustomer(CustomerModel customer)
        {
            MySqlCommand cmd = new MySqlCommand(
                "update tbCustomer set nameCustomer=@nameCustomer, cpfCustomer=@cpfCustomer, phoneCustomer=@phoneCustomer, emailCustomer=@emailCustomer, "
                    + "passwordCustomer=@passwordCustomer where "
                    + "idCustomer=@idCustomer",
                con.ConectarBD()
            );

            cmd.Parameters.AddWithValue("@nameCustomer", customer.nameCustomer);
            cmd.Parameters.AddWithValue("@cpfCustomer", customer.cpfCustomer);
            cmd.Parameters.AddWithValue("@emailCustomer", customer.emailCustomer);
            cmd.Parameters.AddWithValue("@passwordCustomer", customer.passwordCustomer);
            cmd.Parameters.AddWithValue("@phoneCustomer", customer.phoneCustomer);
            cmd.Parameters.AddWithValue("@idCustomer", customer.idCustomer);

            int i = cmd.ExecuteNonQuery();
            con.DesconectarBD();
            if (i >= 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int UpdateAddress(CustomerModel address)
        {
            using (MySqlConnection connection = new MySqlConnection("Server=localhost;DataBase=teste2;User=root;pwd=12345678"))
            {
                connection.Open();

                string query = "UPDATE tbAddressCustomer SET zipCode = @zipCode, streetName = @streetName, " +
                               "streetNumber = @streetNumber, addressComplement = @addressComplement, " +
                               "neighborhood = @neighborhood, city = @city, state = @state " +
                "WHERE idAddress = @idAddress; SELECT LAST_INSERT_ID(); ";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@zipCode", address.zipCode);
                    command.Parameters.AddWithValue("@streetName", address.streetName);
                    command.Parameters.AddWithValue("@streetNumber", address.streetNumber);
                    command.Parameters.AddWithValue("@addressComplement", address.addressComplement);
                    command.Parameters.AddWithValue("@neighborhood", address.neighborhood);
                    command.Parameters.AddWithValue("@city", address.city);
                    command.Parameters.AddWithValue("@state", address.state);
                    command.Parameters.AddWithValue("@idAddress", address.idAddress);

                    idAddress = Convert.ToInt32(command.ExecuteScalar());

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected >= 1)
                    {
                        return idAddress;
                    }
                }
            }
            return idAddress;
        }


        MySqlConnection cn = new MySqlConnection(
            "Server=localhost;DataBase=teste2;User=root;pwd=12345678"
        );

        public string SelectCPFCustomer(string cpf)
        {
            cn.Open();
            MySqlCommand cmd = new MySqlCommand("call spSelectCPFCustomer(@cpfCustomer);");
            cmd.Parameters.Add("@cpfCustomer", MySqlDbType.String).Value = cpf;
            cmd.Connection = cn;
            string CPF = (string)cmd.ExecuteScalar();
            cn.Close();
            if (CPF == null)

                CPF = "";
            return CPF;
        }

        public string SelectEmailCustomer(string email)
        {
            cn.Open();
            MySqlCommand cmd = new MySqlCommand("call spSelectEmailCustomer(@emailCustomer);");
            cmd.Parameters.Add("@emailCustomer", MySqlDbType.String).Value = email;
            cmd.Connection = cn;
            string Email = (string)cmd.ExecuteScalar();
            cn.Close();
            if (Email == null)

                Email = "";
            return Email;
        }



        public int InsertAddress(CustomerModel customer)
        {
            int idAddress = 0;
            cn.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO tbAddressCustomer (zipCode, streetName, streetNumber, addressComplement, neighborhood, city, state) VALUES (@zipCode, @streetName, @streetNumber, @addressComplement, @neighborhood, @city, @state); SELECT LAST_INSERT_ID();", cn);

            cmd.Parameters.AddWithValue("@zipCode", customer.zipCode);
            cmd.Parameters.AddWithValue("@streetName", customer.streetName);
            cmd.Parameters.AddWithValue("@streetNumber", customer.streetNumber);
            cmd.Parameters.AddWithValue("@addressComplement", customer.addressComplement);
            cmd.Parameters.AddWithValue("@neighborhood", customer.neighborhood);
            cmd.Parameters.AddWithValue("@city", customer.city);
            cmd.Parameters.AddWithValue("@state", customer.state);

            // Executar a inserção e obter o último ID inserido
            idAddress = Convert.ToInt32(cmd.ExecuteScalar());

            cn.Close();

            return idAddress;
        }

        public int InsertCustomerWithAddress(CustomerModel customer)
        {
            int idAddress = InsertAddress(customer);
            cn.Open();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO tbCustomer (nameCustomer, cpfCustomer, phoneCustomer, emailCustomer, passwordCustomer, idAddress) VALUES (@nameCustomer, @cpfCustomer, @phoneCustomer, @emailCustomer, @passwordCustomer, @idAddress)");

            cmd.Parameters.AddWithValue("@nameCustomer", customer.nameCustomer);
            cmd.Parameters.AddWithValue("@cpfCustomer", customer.cpfCustomer);
            cmd.Parameters.AddWithValue("@phoneCustomer", customer.phoneCustomer);
            cmd.Parameters.AddWithValue("@emailCustomer", customer.emailCustomer);
            cmd.Parameters.AddWithValue("@passwordCustomer", customer.passwordCustomer);
            cmd.Parameters.AddWithValue("@idAddress", idAddress);

            cmd.Connection = cn;
            cmd.ExecuteNonQuery();

            cn.Close();

            return idAddress;
        }


        public void updateCustomerWithAddress(CustomerModel customer)
        {
            string connectionString = "Server=localhost;DataBase=teste2;User=root;pwd=12345678";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                MySqlCommand cmd = new MySqlCommand("UpdateCustomerWithAddress", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("p_idCustomer", customer.idCustomer);
                cmd.Parameters.AddWithValue("p_nameCustomer", customer.nameCustomer);
                cmd.Parameters.AddWithValue("p_cpfCustomer", customer.cpfCustomer);
                cmd.Parameters.AddWithValue("p_phoneCustomer", customer.phoneCustomer);
                cmd.Parameters.AddWithValue("p_emailCustomer", customer.emailCustomer);
                cmd.Parameters.AddWithValue("p_passwordCustomer", customer.passwordCustomer);
                cmd.Parameters.AddWithValue("p_zipCode", customer.zipCode);
                cmd.Parameters.AddWithValue("p_streetName", customer.streetName);
                cmd.Parameters.AddWithValue("p_streetNumber", customer.streetNumber);
                cmd.Parameters.AddWithValue("p_addressComplement", customer.addressComplement);
                cmd.Parameters.AddWithValue("p_neighborhood", customer.neighborhood);
                cmd.Parameters.AddWithValue("p_city", customer.city);
                cmd.Parameters.AddWithValue("p_state", customer.state);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
