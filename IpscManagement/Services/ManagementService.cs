using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using IpscManagement.Controllers;
using IpscManagement.DB;
using IpscManagement.Models;
using Microsoft.Ajax.Utilities;
using WebGrease.Css.Ast.Selectors;
using Member = IpscManagement.DB.Member;

namespace IpscManagement.Services
{
    public class ManagementService
    {
        public static ActionResultModel SetMember(MemberModel member)
        {
            ActionResultModel actionResultModel = new ActionResultModel();
            using (var context = new IpscManagementEntities())
            {
                if (member.Id != null && member.Id != 0 && context.Members.Any(m => m.Id == member.Id))
                {
                    var editMember = context.Members.FirstOrDefault(m => m.Id == member.Id);
                    editMember.Identity = member.Identity;
                    editMember.FirstName = member.FirstName;
                    editMember.LastName = member.LastName;
                    editMember.FatherName = member.FatherName;
                    editMember.Gender = Int16.Parse(member.Gender);
                    editMember.DateofBirth = Convert.ToDateTime(member.DateofBirthStr);
                    editMember.ArmyId = member.ArmyId;
                    editMember.Email = member.Email;
                    editMember.Address = member.Address;
                    editMember.City = member.City;
                    editMember.Zip = member.Zip;
                    editMember.Phone = member.Phone;
                    editMember.MobilePhone = member.MobilePhone;
                    editMember.ShooterIdentity = member.ShooterIdentity;
                    editMember.Active = member.Active;
                    


                    actionResultModel.Successed = true;

                }
                else
                {
                    if (!context.Members.Any(m => m.Identity == member.Identity))
                    {
                        var newMember = new Member()
                        {
                            Identity = member.Identity,
                            FirstName = member.FirstName,
                            LastName = member.LastName,
                            FatherName = member.FatherName,
                            DateofBirth = Convert.ToDateTime(member.DateofBirthStr),
                            ArmyId = member.ArmyId,
                            Email = member.Email,
                            Address = member.Address,
                            City = member.City,
                            Zip = member.Zip,
                            Phone = member.Phone,
                            MobilePhone = member.MobilePhone,
                            Active = member.Active,
                            DateTime = DateTime.Now,
                            ShooterIdentity = member.ShooterIdentity,
                        };
                        
                        newMember.Gender = Int16.Parse(member.Gender);
                        context.Members.Add(newMember);
                        context.SaveChanges();

                        context.BulletsStocks.Add(new BulletsStock()
                        {
                            MemberId = newMember.Id,
                            MemberIdentity = newMember.Identity,
                            DateTime = DateTime.Now

                        });
                        actionResultModel.Successed = true;
                    }
                    else
                    {
                        actionResultModel.Successed = false;
                        actionResultModel.Message = "Member with same Identity alredy exist";
                    }
                    
                }
                context.SaveChanges();
            }
            return actionResultModel;
        }

        public static void UpdateAmount(AmountChangeModel amountChange, ApplicationUser appUser)
        {
            var bulletsAmountChangeEmailModel = new BulletsAmountChangeEmailModel();
            using (var context = new IpscManagementEntities())
            {
                var bulletsStock = context.BulletsStocks.FirstOrDefault(bs => bs.MemberIdentity == amountChange.MemberIdentity);
                var member = context.Members.FirstOrDefault(m => m.Identity == amountChange.MemberIdentity);
                var warehouseBulletsStock = context.WarehouseBulletsStocks.FirstOrDefault();
                int previousAmmount = bulletsStock.Amount;
                int warehousepreviousAmmount = warehouseBulletsStock.Amount;

                switch (amountChange.AmountUpdateType)
                {
                    case 1:

                        if (!string.IsNullOrWhiteSpace(member.Email) && !string.IsNullOrEmpty(member.Email))
                        {
                            bulletsAmountChangeEmailModel = SetValue(amountChange, member, bulletsStock);
                        }
                        bulletsAmountChangeEmailModel.NewAmount = bulletsStock.Amount += amountChange.Amount;
                        
                        
                        break;

                    case 2:

                        if (!string.IsNullOrWhiteSpace(member.Email) && !string.IsNullOrEmpty(member.Email))
                        {
                            bulletsAmountChangeEmailModel = SetValue(amountChange, member, bulletsStock);
                        }
                        bulletsAmountChangeEmailModel.NewAmount = bulletsStock.Amount -= amountChange.Amount;
                        
                            
                        warehouseBulletsStock.Amount -= amountChange.Amount;

                        context.WarehouseBulletsStockHistories.Add(new WarehouseBulletsStockHistory()
                        {
                            Amount = amountChange.Amount,
                            ActionType = amountChange.AmountUpdateType,
                            MemberId = bulletsStock.MemberId,
                            DateTime = DateTime.Now,
                            Remarks = amountChange.Remarks,
                            Modifier = appUser != null ? appUser.Id : "-1",
                            PreviousAmmount = warehousepreviousAmmount,
                            NewAmmount = warehouseBulletsStock.Amount

                        });

                        break;
                }
                context.BulletsStockHistories.Add(new BulletsStockHistory()
                {
                    Amount = amountChange.Amount,
                    MemberIdentity = amountChange.MemberIdentity,
                    ActionType = amountChange.AmountUpdateType,
                    MemberId = bulletsStock.MemberId,
                    DateTime = DateTime.Now,
                    Remarks = amountChange.Remarks,
                    Modifier = appUser != null ? appUser.Id : "-1",
                    PreviousAmmount = previousAmmount,
                    NewAmmount = bulletsStock.Amount

                });

                context.SaveChanges();
            }
            if (bulletsAmountChangeEmailModel.To.Count > 0)
            {
                bulletsAmountChangeEmailModel.Remarks = amountChange.Remarks;
                SendNotificationEmailService.AmmountChange(bulletsAmountChangeEmailModel);
            }
                
        }

        public static int UpdateWarehouseBulletsStock(WarehouseBulletsStockModel warehouseBulletsStockModel, ApplicationUser appUser)
        {
            int newAmmoAmmount;
            int previousAmmoAmmount = 0;
            bool newAmmountresult = int.TryParse(warehouseBulletsStockModel.Amount.ToString(), out newAmmoAmmount);

            using (var context = new IpscManagementEntities())
            {
                var warehouseBulletsStock = context.WarehouseBulletsStocks.FirstOrDefault();
                previousAmmoAmmount = warehouseBulletsStock.Amount;

                if (newAmmountresult)
                {
                    switch (warehouseBulletsStockModel.AmountUpdateType)
                    {
                        case 1://Remove
                            warehouseBulletsStock.Amount -= newAmmoAmmount;
                            break;
                        case 2://Add
                            warehouseBulletsStock.Amount += newAmmoAmmount;
                            break;
                        case 3://Update
                            warehouseBulletsStock.Amount = newAmmoAmmount;
                            break;

                    }
                }

                context.WarehouseBulletsStockHistories.Add(new WarehouseBulletsStockHistory()
                {
                    Amount = newAmmountresult ? newAmmoAmmount : warehouseBulletsStock.Amount,
                    //MemberIdentity = warehouseBulletsStockModel.MemberIdentity,
                    ActionType = warehouseBulletsStockModel.AmountUpdateType,
                    DateTime = DateTime.Now,
                    Remarks = $"כמות עודכנה מ {previousAmmoAmmount} ל {warehouseBulletsStock.Amount} הערות נוספות: {warehouseBulletsStockModel.Remarks}",
                    Modifier = appUser.Id,
                    PreviousAmmount = previousAmmoAmmount,
                    NewAmmount = warehouseBulletsStock.Amount

                });
                context.SaveChanges();

                newAmmoAmmount = warehouseBulletsStock.Amount;
            }

            return newAmmoAmmount;
        }

        private static BulletsAmountChangeEmailModel SetValue(AmountChangeModel amountChange, Member member, BulletsStock bulletsStock)
        {

            var bulletsAmountChangeEmailModel = new BulletsAmountChangeEmailModel()
            {
                To = {member.Email}
            };

            bulletsAmountChangeEmailModel.FirstName = member.FirstName;
            bulletsAmountChangeEmailModel.LastName = member.LastName;
            bulletsAmountChangeEmailModel.PreviousAmount = bulletsStock.Amount;
            bulletsAmountChangeEmailModel.ChangeAmount = amountChange.Amount;
            return bulletsAmountChangeEmailModel;
            //return new BulletsAmountChangeEmailModel()
            //{
            //    FirstName = member.FirstName,
            //    LastName = member.LastName,
            //    PreviousAmount = bulletsStock.Amount,
            //    ChangeAmount = amountChange.Amount,
            //    To = { member.Email }
            //};    
        }

        public static List<BulletsStockModel> GetStocks()
        {
            var stocks = new List<BulletsStockModel>();
            using (var context = new IpscManagementEntities())
            {
                foreach (var member in context.Members.Where(m => m.Active))
                {
                    stocks.Add(new BulletsStockModel()
                    {
                        FirstName = member.FirstName,
                        LastName = member.LastName,
                        Identity = member.Identity,
                        Amount = context.BulletsStocks.FirstOrDefault(bs => bs.MemberIdentity == member.Identity).Amount,
                        ShooterIdentity = member.ShooterIdentity.GetValueOrDefault()
                        
                    });
                }
            }
            return stocks;
        }

        public static MemberModel GetMemberByIdentity(int identity)
        {
            var memberModel = new MemberModel();
            using (var context = new IpscManagementEntities())
            {
                var selectedMember = context.Members.FirstOrDefault(m => m.Identity == identity);
                memberModel.Id = selectedMember.Id;
                memberModel.Identity = selectedMember.Identity;
                memberModel.FirstName = selectedMember.FirstName;
                memberModel.LastName = selectedMember.LastName;
                memberModel.FatherName = selectedMember.FatherName;
                memberModel.Gender = selectedMember.Gender.ToString();
                memberModel.DateofBirth = selectedMember.DateofBirth;
                memberModel.ArmyId = selectedMember.ArmyId;
                memberModel.Email = selectedMember.Email;
                memberModel.Address = selectedMember.Address;
                memberModel.Zip = selectedMember.Zip;
                memberModel.Phone = selectedMember.Phone;
                memberModel.MobilePhone = selectedMember.MobilePhone;
                memberModel.ShooterIdentity = selectedMember.ShooterIdentity.GetValueOrDefault();
                memberModel.Active = selectedMember.Active;
                memberModel.City = selectedMember.City;

            }
            return memberModel;
        }

        public static List<BulletsStockHistoryModel> GetMemberBulletsStockHistoryChanges(int identity)
        {
            List<BulletsStockHistoryModel> bulletsStockHistoryModels = new List<BulletsStockHistoryModel>();
            using (var context = new IpscManagementEntities())
            {
                var selectedMember = context.BulletsStockHistories.Where(m => m.MemberIdentity == identity);
                foreach (var stockHistory in selectedMember)
                {
                    bulletsStockHistoryModels.Add(new BulletsStockHistoryModel()
                    {
                        DateTime = stockHistory.DateTime,
                        Amount = stockHistory.Amount,
                        ActionType = stockHistory.ActionType == 1 ? "הוספה" : "הסרה",
                        Remarks = stockHistory.Remarks,
                        PreviousAmmount = stockHistory.PreviousAmmount,
                        NewAmmount = stockHistory.NewAmmount
                    });
                }
            }

            return bulletsStockHistoryModels;
        }

        public static int GetWarehouseBulletsStockAmmount()
        {
            int ammoAmmount;
            using (var context = new IpscManagementEntities())
            {
                ammoAmmount = context.WarehouseBulletsStocks.FirstOrDefault().Amount;
            }

            return ammoAmmount;
        }

        public static List<WarehouseStockHistoryChangeModel> GetWarehouseStockHistoryChanges()
        {
            var warehouseStockHistoryChanges = new List<WarehouseStockHistoryChangeModel>();

            using (var context = new IpscManagementEntities())
            {
                var warehouseHistory = context.WarehouseBulletsStockHistories;
                foreach (var warehouseHistoryRecord in warehouseHistory)
                {
                    var warehouseStockHistoryChangeModel = new WarehouseStockHistoryChangeModel
                    {
                        Amount = warehouseHistoryRecord.Amount,
                        DateTime = warehouseHistoryRecord.DateTime,
                        Remarks = warehouseHistoryRecord.Remarks,
                        PreviousAmmount = warehouseHistoryRecord.PreviousAmmount,
                        NewAmmount = warehouseHistoryRecord.NewAmmount
                    };

                    var memeber = context.Members.FirstOrDefault(m => m.Id == warehouseHistoryRecord.MemberId);
                    if (memeber != null)
                    {
                        warehouseStockHistoryChangeModel.Name = $"{memeber.FirstName} {memeber.LastName}";
                    }

                    if (warehouseHistoryRecord.ActionType == 1)
                    {
                        warehouseStockHistoryChangeModel.ActionType = "הסרה";
                    }
                    if (warehouseHistoryRecord.ActionType == 2)
                    {
                        warehouseStockHistoryChangeModel.ActionType = "הוספה";
                    }
                    if (warehouseHistoryRecord.ActionType == 3)
                    {
                        warehouseStockHistoryChangeModel.ActionType = "עדכון";
                    }
                    warehouseStockHistoryChanges.Add(warehouseStockHistoryChangeModel);
                }
            }
            return warehouseStockHistoryChanges;
        }

        public static List<BulletsStockHistoryModel> GetMemberBulletsStockHistoryChangesForCSV(string identity, DateTime fromDate, DateTime toDate)
        {
            List<BulletsStockHistoryModel> bulletsStockHistoryModels = new List<BulletsStockHistoryModel>();

            int id;
            //DateTime fDate = new DateTime();
            //DateTime tDate = new DateTime();
            if (Int32.TryParse(identity, out id))
            {
                //if (!DateTime.TryParse(fromDate, out fDate))
                //{
                //    fDate = DateTime.Now.AddYears(-100);
                //}
                //if (!DateTime.TryParse(toDate, out tDate))
                //{
                //    tDate = DateTime.Now.AddDays(1);
                //}

                using (var context = new IpscManagementEntities())
                {
                    var selectedMember = context.BulletsStockHistories.Where(m => m.MemberIdentity == id && m.DateTime > fromDate && m.DateTime < toDate).OrderByDescending(m => m.DateTime);

                    bulletsStockHistoryModels.AddRange(selectedMember.Select(stockHistory => new BulletsStockHistoryModel()
                    {
                        DateTime = stockHistory.DateTime,
                        Amount = stockHistory.Amount,
                        ActionType = stockHistory.ActionType == 1 ? "הוספה" : "הסרה",
                        NewAmmount = stockHistory.NewAmmount,
                        PreviousAmmount = stockHistory.PreviousAmmount,
                        Remarks = stockHistory.Remarks
                    }));
                }
            }

            return bulletsStockHistoryModels;
        }

        public static List<WarehouseStockHistoryChangeModel> GetWarehouseStockHistoryChangesReport(DateTime fromDate, DateTime toDate)
        {
            var warehouseStockHistoryChanges = new List<WarehouseStockHistoryChangeModel>();
            //DateTime fDate = new DateTime();
            //DateTime tDate = new DateTime();
            //if (!DateTime.TryParse(fromDate, out fDate))
            //{
            //    fDate = DateTime.Now.AddYears(-100);
            //}
            //if (!DateTime.TryParse(toDate, out tDate))
            //{
            //    tDate = DateTime.Now.AddDays(1);
            //}
            System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("he-IL");
            using (var context = new IpscManagementEntities())
            {
                var warehouseHistory = context.WarehouseBulletsStockHistories.Where(wh => wh.DateTime >= fromDate && wh.DateTime <= toDate).OrderByDescending(m => m.DateTime);
                foreach (var warehouseHistoryRecord in warehouseHistory)
                {
                    var warehouseStockHistoryChangeModel = new WarehouseStockHistoryChangeModel
                    {
                        Amount = warehouseHistoryRecord.Amount,
                        DateTime = DateTime.Parse(warehouseHistoryRecord.DateTime.ToString(), cultureinfo),
                        Remarks = warehouseHistoryRecord.Remarks,
                        PreviousAmmount = warehouseHistoryRecord.PreviousAmmount,
                        NewAmmount = warehouseHistoryRecord.NewAmmount
                    };

                    var memeber = context.Members.FirstOrDefault(m => m.Id == warehouseHistoryRecord.MemberId);
                    if (memeber != null)
                    {
                        warehouseStockHistoryChangeModel.Name = $"{memeber.FirstName} {memeber.LastName}";
                    }

                    if (warehouseHistoryRecord.ActionType == 1)
                    {
                        warehouseStockHistoryChangeModel.ActionType = "הוספה";
                    }
                    if (warehouseHistoryRecord.ActionType == 2)
                    {
                        warehouseStockHistoryChangeModel.ActionType = "הסרה";
                    }
                    if (warehouseHistoryRecord.ActionType == 3)
                    {
                        warehouseStockHistoryChangeModel.ActionType = "עדכון";
                    }
                    warehouseStockHistoryChanges.Add(warehouseStockHistoryChangeModel);
                }
            }
            return warehouseStockHistoryChanges;
        }

        public static List<ReportModel3Columns> GetWarehouseStocStatusReport(DateTime forDate)
        {
            var reportResult = new List<ReportModel3Columns>();
            
            System.Globalization.CultureInfo cultureinfo = new System.Globalization.CultureInfo("he-IL");
            using (var context = new IpscManagementEntities())
            {
                var warehouseHistory = context.WarehouseBulletsStockHistories.Where(wh => wh.DateTime <= forDate).OrderByDescending(m => m.DateTime).First();
                int totalSum = 0;

                foreach (var member in context.Members.Where(m => m.Active))
                {
                    var selectedMember = context.BulletsStockHistories.Where(
                        m => m.DateTime <= forDate && m.NewAmmount > 0 && m.MemberIdentity == member.Identity).
                        OrderByDescending(m => m.DateTime);

                    if (selectedMember.Count() > 0)
                    {
                        reportResult.Add(new ReportModel3Columns
                        {
                            Column1 = member.FirstName + " " + member.LastName,
                            Column2 = selectedMember.First().MemberIdentity.ToString(),
                            Column3 = selectedMember.First().NewAmmount.ToString()
                        });
                        totalSum = totalSum + selectedMember.First().NewAmmount;
                    }
                }
                 
                reportResult.Add( new ReportModel3Columns()
                {
                    Column1 = "כל יורים",
                    Column2 = "סהכ",
                    Column3 = totalSum.ToString()
                });

                reportResult.Add(new ReportModel3Columns()
                {
                    Column1 = "מצב מחסן פיזי",
                    Column2 = "סהכ",
                    Column3 = warehouseHistory.NewAmmount.ToString()
                });

                reportResult.Add(new ReportModel3Columns()
                {
                    Column1 = "מצב מחסן בפועל",
                    Column2 = "סהכ",
                    Column3 = (warehouseHistory.NewAmmount - totalSum).ToString()
                });
            }
            return reportResult;
        }

    }
}