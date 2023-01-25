using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;
using RunGroupWebApp.ViewModels;

namespace RunGroupWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClubController(IClubRepository clubRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _clubRepository = clubRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubRepository.GetAll();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Club club =  await _clubRepository.GetClubById(id);
            return View(club);
        }

        public IActionResult Create()
        {
            // calling the GetUserId from ClaimsPrincipalExtensions to only get userId and not the whole object
            var currentUserId = _httpContextAccessor.HttpContext.User.GetUserId();
            var createClubViewModel = new CreateClubViewModel()
            {
                AppUserId = currentUserId,
            };
            return View(createClubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel newClub)
        {
            if(ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(newClub.Image);
                var club = new Club
                {
                    AppUserId = newClub.AppUserId,
                    Title = newClub.Title,
                    Description = newClub.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        City = newClub.Address.City,
                        State = newClub.Address.State,
                        Street = newClub.Address.Street
                    }
                };
                _clubRepository.Add(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(newClub);

        }

        public async Task<IActionResult> Edit(int id)
        {
            var club = await _clubRepository.GetClubById(id);

            if (club == null) return View("Error");

            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                ClubCategory = club.ClubCategory,
                Address = club.Address,
                AddressId = club.AddressId,
                URL = club.Image
            };
            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubVM);
            }

            var userClub = await _clubRepository.GetClubByIdNoTracking(id);

            if(userClub != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(clubVM);
                }
                var photoResult = await _photoService.AddPhotoAsync(clubVM.Image);
                var club = new Club
                {
                    Id = id,
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = photoResult.Url.ToString(),
                    AddressId = clubVM.AddressId,
                    Address = clubVM.Address
                };

                _clubRepository.Update(club);
                return RedirectToAction("Index");
            }
            else
            {
                return View(clubVM);
            }
            
        }

        public async Task<IActionResult> Delete(int id)
        {
            var clubDetails = await _clubRepository.GetClubById(id);
            if (clubDetails == null) return View("Error");

            return View(clubDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var clubDetails = await _clubRepository.GetClubById(id);
            if (clubDetails == null) return View("Error");

            _clubRepository.Delete(clubDetails);
            return RedirectToAction("Index");
        }

    }
}
