using Dapper;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;

namespace Ki_ADAS.DB
{
    public class ModelRepository
    {
        private SettingConfigDb db;

        public ModelRepository(SettingConfigDb database)
        {
            db = database;
        }

        public List<Model> GetAllModels()
        {
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    const string query = "SELECT * FROM Model ORDER BY Name";
                    return con.Query<Model>(query).ToList();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorFetchingModels", "DatabaseError", ex.Message);
                return new List<Model>();
            }
        }

        public bool DeleteModel(string modelName)
        {
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    const string query = "DELETE FROM Model WHERE Name = ?";
                    int rowsAffected = con.Execute(query, new { Name = modelName });
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorDeletingModel", "DatabaseError", ex.Message);
                return false;
            }
        }

        public Model GetModelDetails(string modelName)
        {
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    const string query = "SELECT * FROM Model WHERE Name = ?";
                    return con.QueryFirstOrDefault<Model>(query, new { Name = modelName });
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorFetchingModelDetails", "DatabaseError", ex.Message);
                return null;
            }
        }

        public bool AddModel(Model model)
        {
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    if (IsDuplicateName(model.Name))
                    {
                        MsgBox.Warn("ModelNameAlreadyExists");
                        return false;
                    }

                    const string query = @"INSERT INTO Model (
                                        Name, Barcode, Wheelbase, FC_Distance, FC_Height, FC_InterDistance,
                                        FC_Htu, FC_Htl, FC_Ts, FC_AlignmentAxeOffset, FC_Vv, FC_StCt, FC_IsTest,
                                        FR_X, FR_Y, FR_Z, FR_Angle, FL_X, FL_Y, FL_Z, FL_Angle, F_IsTest,
                                        RR_X, RR_Y, RR_Z, RR_Angle, RL_X, RL_Y, RL_Z, RL_Angle, R_IsTest
                                       ) VALUES (
                                        @Name, @Barcode, @Wheelbase, @FC_Distance, @FC_Height, @FC_InterDistance,
                                        @FC_Htu, @FC_Htl, @FC_Ts, @FC_AlignmentAxeOffset, @FC_Vv, @FC_StCt, @FC_IsTest,
                                        @FR_X, @FR_Y, @FR_Z, @FR_Angle, @FL_X, @FL_Y, @FL_Z, @FL_Angle, @F_IsTest,
                                        @RR_X, @RR_Y, @RR_Z, @RR_Angle, @RL_X, @RL_Y, @RL_Z, @RL_Angle, @R_IsTest
                                       )";

                    return con.Execute(query, model) > 0;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorAddingModel", "DatabaseError", ex.Message);
                return false;
            }
        }

        public bool UpdateModel(Model model, string oldModelName)
        {
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    if (oldModelName != model.Name && IsDuplicateName(model.Name, oldModelName))
                    {
                        MsgBox.Warn("ModelNameAlreadyExists");
                        return false;
                    }

                    const string query = @"UPDATE Model SET
                                        Name = @Name, Barcode = @Barcode, Wheelbase = @Wheelbase, FC_Distance = @FC_Distance, FC_Height = @FC_Height, FC_InterDistance = @FC_InterDistance,
                                        FC_Htu = @FC_Htu, FC_Htl = @FC_Htl, FC_Ts = @FC_Ts, FC_AlignmentAxeOffset = @FC_AlignmentAxeOffset, FC_Vv = @FC_Vv, FC_StCt = @FC_StCt, FC_IsTest = @FC_IsTest,
                                        FR_X = @FR_X, FR_Y = @FR_Y, FR_Z = @FR_Z, FR_Angle = @FR_Angle, FL_X = @FL_X, FL_Y = @FL_Y, FL_Z = @FL_Z, FL_Angle = @FL_Angle, F_IsTest = @F_IsTest,
                                        RR_X = @RR_X, RR_Y = @RR_Y, RR_Z = @RR_Z, RR_Angle = @RR_Angle, RL_X = @RL_X, RL_Y = @RL_Y, RL_Z = @RL_Z, RL_Angle = @RL_Angle, R_IsTest = @R_IsTest
                                        WHERE Name = @OldName";

                    return con.Execute(query, new { 
                        model.Name, model.Barcode, model.Wheelbase, model.FC_Distance, model.FC_Height, model.FC_InterDistance,
                        model.FC_Htu, model.FC_Htl, model.FC_Ts, model.FC_AlignmentAxeOffset, model.FC_Vv, model.FC_StCt, model.FC_IsTest,
                        model.FR_X, model.FR_Y, model.FR_Z, model.FR_Angle, model.FL_X, model.FL_Y, model.FL_Z, model.FL_Angle, model.F_IsTest,
                        model.RR_X, model.RR_Y, model.RR_Z, model.RR_Angle, model.RL_X, model.RL_Y, model.RL_Z, model.RL_Angle, model.R_IsTest,
                        OldName = oldModelName 
                    }) > 0;
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorUpdatingModel", "DatabaseError", ex.Message);
                return false;
            }
        }

        public bool IsDuplicateName(string name, string oldName = null)
        {
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    if (string.IsNullOrEmpty(oldName))
                    {
                        const string query = "SELECT COUNT(*) FROM Model WHERE Name = ?";
                        int count = con.ExecuteScalar<int>(query, new { Name = name });
                        return count > 0;
                    }
                    else
                    {
                        const string query = "SELECT COUNT(*) FROM Model WHERE Name = ? AND Name <> ?";
                        int count = con.ExecuteScalar<int>(query, new { NewName = name, OldName = oldName });
                        return count > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorCheckingDuplicateModelName", "DatabaseError", ex.Message);
                return true;
            }
        }

        public string GetModelNameByBarcode(string modelCode)
        {
            try
            {
                using (var con = new OleDbConnection(db.connectionString))
                {
                    const string query = "SELECT Name FROM Model WHERE Barcode = ?";
                    return con.QueryFirstOrDefault<string>(query, new { Barcode = modelCode });
                }
            }
            catch (Exception ex)
            {
                MsgBox.ErrorWithFormat("ErrorFetchingModelNameByBarcode", "DatabaseError", ex.Message);
                return null;
            }
        }
    }
}