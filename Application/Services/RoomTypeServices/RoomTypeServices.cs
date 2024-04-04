using Application.DTOs;
using AutoMapper;
using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Application.Services.RoomTypeServices
{
    public class RoomTypeServices(IRoomTypeRepository _roomTypeRepository, IMapper _mapper) : IRoomTypeServices
    {
        public async Task<IEnumerable<RoomTypeDto>> GetAllRoomTypesAsync()
        {
            var roomTypes = await _roomTypeRepository.GetRoomTypesAsync();
            return _mapper.Map<IEnumerable<RoomTypeDto>>(roomTypes);
        }

        public async Task<RoomTypeDto> GetRoomTypeByIdAsync(Guid Id)
        {
            var roomType = await _roomTypeRepository.GetRoomTypeByIdAsync(Id);
            return _mapper.Map<RoomTypeDto>(roomType);
        }

        public async Task<RoomTypeDto> CreateAsync(RoomTypeDto roomTypeDto, List<IFormFile> photos)
        {
            if (roomTypeDto == null)
            {
                throw new ArgumentNullException(nameof(roomTypeDto));
            }

            var roomType = _mapper.Map<RoomType>(roomTypeDto);
            await AddPhotosToRoomTypeAsync(roomType, photos);

            await _roomTypeRepository.Add(roomType);
            return _mapper.Map<RoomTypeDto>(roomType);
        }

        public async Task UpdateAsync(Guid id, RoomTypeDto roomTypeDto)
        {
            var existingRoomType = await _roomTypeRepository.GetRoomTypeByIdAsync(id);
            _mapper.Map(roomTypeDto, existingRoomType);
            await _roomTypeRepository.UpdateAsync(id, existingRoomType);
        }

        public async Task DeleteAsync(Guid Id)
        {
            var roomType = await _roomTypeRepository.GetRoomTypeByIdAsync(Id);
            await _roomTypeRepository.Delete(roomType);
        }

        private async Task AddPhotosToRoomTypeAsync(RoomType roomType, List<IFormFile> photos)
        {
            if (photos != null && photos.Count > 0)
            {
                roomType.Photos = new List<RoomTypePhoto>();

                foreach (var photo in photos)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await photo.CopyToAsync(memoryStream);
                        roomType.Photos.Add(new RoomTypePhoto
                        {
                            PhotoData = memoryStream.ToArray(),
                            ContentType = photo.ContentType
                        });
                    }
                }
            }
        }
    }
}
