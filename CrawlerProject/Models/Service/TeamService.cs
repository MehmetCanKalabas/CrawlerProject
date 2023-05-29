using Castle.Components.DictionaryAdapter;
using CrawlerProject.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CrawlerProject.Models.Service
{
    public class TeamService
    {
        private Guid AddTeam(AddOrUpdateTeamModel model)
        {
            SqlConnection sqlconnection = ConnectionSettingService.connection;
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.Connection = sqlconnection;
            var newGuid = Guid.NewGuid();
            sqlcommand.CommandText = "INSERT INTO Teams (Id, IsDelete, CreatedDate, TeamName, Logo) VALUES (@Id, @IsDelete, @CreatedDate, @TeamName, @Logo)";
            sqlcommand.Parameters.AddWithValue("@TeamName", model.TeamName);

            var fileName = newGuid + Path.GetExtension(model.Logo.FileName);
            
            sqlcommand.Parameters.AddWithValue("@Logo", "uploads/" + fileName); // LOGO null gitmiyor sonra bak.
            sqlcommand.Parameters.AddWithValue("@IsDelete", false);
            sqlcommand.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
            sqlcommand.Parameters.AddWithValue("@Id", newGuid);

            sqlconnection.Open();
            sqlcommand.ExecuteNonQuery();
            sqlconnection.Close();

            return newGuid;
        }

        public Guid? AddOrUpdateTeam(AddOrUpdateTeamModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Id.ToString()))
            {
                return AddTeam(model);
                
            }
            else
            {
                SqlConnection sqlconnection = ConnectionSettingService.connection;
                SqlCommand sqlcommand = new SqlCommand();
                sqlcommand.Connection = sqlconnection;
                sqlcommand.CommandText = "UPDATE Teams SET TeamName = @TeamName, UpdatedDate = @UpdatedDate WHERE Id=@Id";
                sqlcommand.Parameters.AddWithValue("@TeamName", model.TeamName);
                //sqlcommand.Parameters.AddWithValue("@Logo", model.Logo ?? string.Empty); // LOGO null gitmiyor sonra bak.

                sqlcommand.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                sqlcommand.Parameters.AddWithValue("@Id", model.Id);

                sqlconnection.Open();
                sqlcommand.ExecuteNonQuery();
                sqlconnection.Close();

                return model.Id;
            }
        }
        public bool UpdatePlayer()
        {
            return true;
        }
        public List<Team> GetListTeam()
        {
            SqlConnection sqlconnection = ConnectionSettingService.connection;
            SqlCommand sqlcommand = new SqlCommand();
            sqlconnection.Open();

            sqlcommand.CommandText = "SELECT Id,TeamName,Logo FROM Teams WHERE IsDelete = 0 ORDER BY CreatedDate ASC";
            sqlcommand.Connection = sqlconnection;
            SqlDataReader sqlDataReader = sqlcommand.ExecuteReader();
            List<Team> teamList = new List<Team>();

            while (sqlDataReader.Read())
            {
                Team team = new Team();
                team.Id = (Guid)sqlDataReader[0];
                team.Name = sqlDataReader[1].ToString();
                team.Logo = sqlDataReader[2].ToString();
                teamList.Add(team);
            }

            return teamList;
        }
        public List<GetPlayerName> GetTeamPlayer(ObjectItem param)
        {
            SqlConnection sqlconnection = ConnectionSettingService.connection;
            SqlCommand sqlcommand = new SqlCommand();
            sqlconnection.Open();

            sqlcommand.CommandText = "select PlayerName, LastName from Players Where IsDelete = 0 and TeamId = @Id";
            sqlcommand.Parameters.AddWithValue("@Id", param.Id);
            sqlcommand.Connection = sqlconnection;
            SqlDataReader sqlDataReader = sqlcommand.ExecuteReader();
            List<GetPlayerName> responseList = new List<GetPlayerName>();

            while (sqlDataReader.Read())
            {
                GetPlayerName response = new GetPlayerName();
                response.PlayerName = string.Concat(sqlDataReader[0].ToString(), " ", sqlDataReader[1].ToString());
                responseList.Add(response);
            }

            return responseList;
        }
        public static string GetTeamName(ObjectItem param)
        {
            string result = null;
            SqlConnection sqlconnection = ConnectionSettingService.connection;
            SqlCommand sqlcommand = new SqlCommand();
            sqlconnection.Open();

            sqlcommand.CommandText = "select TeamName from Teams Where Id = @Id";
            sqlcommand.Parameters.AddWithValue("@Id", param.Id);
            sqlcommand.Connection = sqlconnection;
            SqlDataReader sqlDataReader = sqlcommand.ExecuteReader();
            List<Team> responseList = new List<Team>();

            while (sqlDataReader.Read())
            {
                result = sqlDataReader[0].ToString();
            }

            return result;
        }
        public SqlDataReader GetByIdPlayer(Guid id)
        {
            SqlConnection sqlconnection = ConnectionSettingService.connection;
            SqlCommand sqlcommand = new SqlCommand();
            sqlconnection.Open();

            sqlcommand.CommandText = "select*from Players Where Id = '@GuidId'";
            sqlcommand.Parameters.AddWithValue("@GuidId", id);
            sqlcommand.Connection = sqlconnection;
            SqlDataReader sdr = sqlcommand.ExecuteReader();
            sqlconnection.Close();
            return sdr;
        }

        public bool DeleteTeamById(Guid id)
        {
            SqlConnection sqlconnection = ConnectionSettingService.connection;
            SqlCommand sqlcommand = new SqlCommand();
            sqlcommand.Connection = sqlconnection;
           
            sqlcommand.CommandText = "DELETE FROM Teams WHERE Id = @Id";
            sqlcommand.Parameters.AddWithValue("@Id", id);
            
            sqlconnection.Open();
            sqlcommand.ExecuteNonQuery();
            sqlconnection.Close();
            return true;
        }
    }

    
}
