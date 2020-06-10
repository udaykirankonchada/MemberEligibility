using MemberEligibility.CustomModel;
using MemberEligibility.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MemberEligibility.Controllers
{
    [RoutePrefix("api/MemberEligibility")]
    public class ValuesController : ApiController
    {
        #region MemberSearchByTechnologyID
        /// <summary>
        /// Description : Based on the technologyID to get details of member details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("MemberSearch"), HttpGet]
        public string MemberSearchByTechnologyID(int technologyID)
        {
            try
            {
                List<MemberModelClass> objList = new List<MemberModelClass>();
                using (MemberEligibilityEntities _db = new MemberEligibilityEntities())
                {
                    objList = (from memberSearchValue in _db.MemberEntities
                               where memberSearchValue.TechnologyID == technologyID
                               select new MemberModelClass
                               {
                                   MemberID = memberSearchValue.MemberID,
                                   MemberName = memberSearchValue.MemberName
                               }).ToList();

                    var jsonobj = JsonConvert.SerializeObject(objList);
                    return jsonobj;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region MemberDetailsByMemberID
        /// <summary>
        /// Description : Based on the member ID to get details of member
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("MemberDetails"), HttpGet]
        public string MemberDetailsByMemberID(int memberID)
        {
            try
            {
                using (MemberEligibilityEntities _db = new MemberEligibilityEntities())
                {
                    var memberObj = (from memberDetails in _db.MemberEntities
                                     where memberDetails.MemberID == memberID
                                     select new MemberEntityModel
                                     {
                                         MemberID = memberDetails.MemberID,
                                         TechnologyID = memberDetails.TechnologyID,
                                         MemberName = memberDetails.MemberName,
                                         DateOfBirth = memberDetails.DateOfBirth,
                                         Qualification = memberDetails.Qualification,
                                         YearsOfExperience = memberDetails.YearsOfExperience,
                                         Technology = memberDetails.TechnologyEntity.TechnologyName
                                     }).FirstOrDefault();

                    var jsonobj = JsonConvert.SerializeObject(memberObj);
                    return jsonobj;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetAllMemberDetails
        /// <summary>
        /// Description : get all member deatils for grid binding
        /// </summary>
        /// <returns></returns>
        [Route("GetAllMemberDetails"), HttpGet]
        public List<MemberEntityModel> GetAllMemberDetails()
        {
            List<MemberEntityModel> memberDetails = new List<MemberEntityModel>();
            try
            {
                using (MemberEligibilityEntities _db = new MemberEligibilityEntities())
                {
                    memberDetails = (from memberSearchValue in _db.MemberEntities
                                     select new MemberEntityModel
                                     {
                                         MemberID = memberSearchValue.MemberID,
                                         TechnologyID = memberSearchValue.TechnologyID,
                                         MemberName = memberSearchValue.MemberName,
                                         DateOfBirth = memberSearchValue.DateOfBirth,
                                         Qualification = memberSearchValue.Qualification,
                                         YearsOfExperience = memberSearchValue.YearsOfExperience,
                                         Technology = memberSearchValue.TechnologyEntity.TechnologyName
                                     }).ToList();

                    if (memberDetails != null)
                    {
                        foreach (var item in memberDetails)
                        {
                            string dateOFBirthd = item.DateOfBirth != null ? item.DateOfBirth.Value.ToString("dd/MM/yyyy") : " ";
                            item.DOB = dateOFBirthd;
                        }
                    }
                    return memberDetails;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion



        #region DeleteMemberRecordByID
        /// <summary>
        /// Description : Based on the member ID delete member record
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        [Route("DeleteMemberDetails"), HttpGet]
        public dynamic DeleteMemberRecordByID(int memberId)
        {
            List<MemberEntityModel> memberDetails = new List<MemberEntityModel>();
            try
            {
                using (MemberEligibilityEntities _db = new MemberEligibilityEntities())
                {
                    var memberAddEntity = _db.MemberEntities.Where(x => x.MemberID == memberId).FirstOrDefault();
                    _db.MemberEntities.Remove(memberAddEntity);
                    int iResult = _db.SaveChanges();
                    if (iResult > 0)
                    {
                        return new
                        {
                            Valid = true,
                            Message = "Member removed successfully"
                        };
                    }
                    return memberDetails;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region GetAllTechnologies
        /// <summary>
        /// Description : LookUp Get All Technologies 
        /// </summary>
        /// <returns></returns>
        [Route("GetAllTechnologies"), HttpGet]
        public List<TechnologyEntityModel> GetAllTechnologies()
        {
            List<TechnologyEntityModel> technologiesObj = new List<TechnologyEntityModel>();
            try
            {
                using (MemberEligibilityEntities _db = new MemberEligibilityEntities())
                {
                    technologiesObj = (from technologies in _db.TechnologyEntities
                                       select new TechnologyEntityModel
                                       {
                                           TechnologyID = technologies.TechnologyID,
                                           TechnologyName = technologies.TechnologyName
                                       }).ToList();

                    return technologiesObj;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region SaveMethod
        /// <summary>
        /// Description : Save member records
        /// </summary>
        /// <param name="MemberEntityModel"></param>
        /// <returns></returns>
        [Route("SaveMemberDetails"), HttpPost]
        public dynamic SaveMethod(MemberEntityModel MemberEntityModel)
        {
            List<TechnologyEntityModel> memberDetails = new List<TechnologyEntityModel>();
            try
            {
                using (MemberEligibilityEntities _db = new MemberEligibilityEntities())
                {
                    //Time compare
                    int compareDates = DateTime.Compare(DateTime.Now, MemberEntityModel.DateOfBirth.Value);

                    if (compareDates < 0)
                    {
                        return new
                        {
                            Valid = false,
                            Message = "Selected date must be greater than today's date."
                        };
                    }

                    int ageDiff = new DateTime((DateTime.Now - MemberEntityModel.DateOfBirth.Value).Ticks).Year;
                    if (ageDiff < 25)
                    {
                        return new
                        {
                            Valid = false,
                            Message = "Date of birth greater than or equal to 25 years is valid."
                        };
                    }
                    if (MemberEntityModel.YearsOfExperience < 3)
                    {
                        return new
                        {
                            Valid = false,
                            Message = "Years Of Experience must be greater than or equal to 3."
                        };
                    }

                    //Check result is valid or not
                    var memberAddEntity = _db.MemberEntities.Where(x => x.MemberID == MemberEntityModel.MemberID).FirstOrDefault();
                    if (memberAddEntity == null)
                    {
                        MemberEntity newMemberEntity = new MemberEntity();
                        newMemberEntity.MemberName = MemberEntityModel.MemberName;
                        newMemberEntity.Qualification = MemberEntityModel.Qualification;
                        newMemberEntity.TechnologyID = MemberEntityModel.TechnologyID;
                        newMemberEntity.DateOfBirth = MemberEntityModel.DateOfBirth.Value;
                        newMemberEntity.Qualification = MemberEntityModel.Qualification;
                        newMemberEntity.YearsOfExperience = MemberEntityModel.YearsOfExperience;
                        _db.MemberEntities.Add(newMemberEntity);
                        int iResult = _db.SaveChanges();
                        if (iResult > 0)
                        {
                            return new
                            {
                                Valid = true,
                                Message = "Member created Succesully"
                            };
                        }
                        else
                        {
                            return new
                            {
                                Valid = false,
                                Message = "Member created Succesully"
                            };
                        }
                    }
                    else
                    {
                        memberAddEntity.MemberName = MemberEntityModel.MemberName;
                        memberAddEntity.Qualification = MemberEntityModel.Qualification;
                        memberAddEntity.TechnologyID = MemberEntityModel.TechnologyID;
                        memberAddEntity.DateOfBirth = MemberEntityModel.DateOfBirth.Value;
                        memberAddEntity.Qualification = MemberEntityModel.Qualification;
                        memberAddEntity.YearsOfExperience = MemberEntityModel.YearsOfExperience;
                        int iResult = _db.SaveChanges();
                        if (iResult > 0)
                        {
                            return new
                            {
                                Valid = true,
                                Message = "Member updated Succesully"
                            };
                        }
                        else
                        {
                            return new
                            {
                                Valid = true,
                                Message = "Member updated Succesully"
                            };
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                return new
                {
                    Valid = false,
                    Message = ex.Message.ToString()
                };
                throw ex;
            }
        }
        #endregion
    }
}
