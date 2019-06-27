using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentMaster.Data;
using StudentMaster.Models;
using StudentMaster.ViewModels;
using StudentMaster.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentMaster.Services
{
    public class AdminService
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly UserManager<User> userManager;
        private readonly IMapper mapper;

        public AdminService(ApplicationDbContext applicationDbContext, UserManager<User> userManager, IMapper mapper)
        {
            this.mapper = mapper;
            this.userManager = userManager;
            this.applicationDbContext = applicationDbContext;
        }
        public async Task<bool> EditUser(EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null || !(await userManager.IsEmailConfirmedAsync(user)))
            {
                return false;
            }

            user.BirthDate = model.BirthDate;
            user.Email = model.Email;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.BirthDate = model.BirthDate;

            await userManager.UpdateAsync(user);
            
            applicationDbContext.SaveChanges();

            return true;
        }

        public async Task<UserListViewModel> GetUsers(string name, int page = 1,
           SortState sortOrder = SortState.FirstNameAsc)
        {
            int pageSize = 5;

            IQueryable<User> AllUsers = userManager.Users;

            //Filtring
            if (!String.IsNullOrEmpty(name))
            {
                AllUsers = AllUsers.Where(p => p.FirstName.Contains(name) || p.LastName.Contains(name));
            }

            //Sorting
            switch (sortOrder)
            {
                case SortState.FirstNameAsc:
                    AllUsers = AllUsers.OrderBy(s => s.FirstName);
                    break;
                case SortState.FirstNameDesc:
                    AllUsers = AllUsers.OrderByDescending(s => s.FirstName);
                    break;
                case SortState.LastNameAsc:
                    AllUsers = AllUsers.OrderBy(s => s.LastName);
                    break;
                case SortState.LastNameDesc:
                    AllUsers = AllUsers.OrderByDescending(s => s.LastName);
                    break;
                case SortState.AgeAsc:
                    AllUsers = AllUsers.OrderByDescending(s => s.BirthDate);
                    break;
                case SortState.AgeDesc:
                    AllUsers = AllUsers.OrderBy(s => s.BirthDate);
                    break;
                case SortState.RegistredDateAsc:
                    AllUsers = AllUsers.OrderBy(s => s.RegistrationDate);
                    break; 
                case SortState.RegistredDateDesc:
                    AllUsers = AllUsers.OrderByDescending(s => s.RegistrationDate);
                    break;
                case SortState.StudyDateAsc:
                    AllUsers = AllUsers.OrderBy(s => s.StudyDate);
                    break;
                case SortState.StudyDateDesc:
                    AllUsers = AllUsers.OrderByDescending(s => s.StudyDate);
                    break;
                default:
                    AllUsers = AllUsers.OrderBy(s => s.FirstName);
                    break;
            }

            //Pagination
            var count = await AllUsers.CountAsync();
            var items = await AllUsers.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var mappedList = new List<UserViewModel>();

            //Maping RecipeList
            foreach (var el in items)
            {
                mappedList.Add(mapper.Map<UserViewModel>(el));
            }

            //Creating Model for client
            UserListViewModel viewModel = new UserListViewModel
            {
                
                PageViewModel = new PageViewModel(count, page, pageSize),
                SortViewModel = new SortViewModel(sortOrder),
                FilterViewModel = new FilterViewModel(name),
                Users = mappedList
            };

            return viewModel;

        }
    }
}

