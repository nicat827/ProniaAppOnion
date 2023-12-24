using FluentValidation;
using ProniaOnion.Application.Abstractions.Repositories;
using ProniaOnion.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion.Application.Validators.Product
{
    public sealed class ProductPostDtoValidator:AbstractValidator<ProductPostDto>
    {
        private const string REQUIRED_MESS = "The {PropertyName} field is required!";
        private const string MAXLENGTH_MESS = "The {PropertyName} field length must be less or equal than {MaxLength}!";
        private const string MINLENGTH_MESS = "The {PropertyName} field length must be greater or equal than {MinLength}!";

       
        public ProductPostDtoValidator()
        {
            
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(REQUIRED_MESS)
                .MaximumLength(100).WithMessage(MAXLENGTH_MESS)
                .MinimumLength(2).WithMessage(MINLENGTH_MESS);
              
                //.MustAsync(_isExist);
           
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage(MAXLENGTH_MESS);
            RuleFor(x => x.Price)
                .Must(p => p > 0 && p < 999999.99m).WithMessage("Price is not valid!");
            RuleFor(x => x.CategoryId)
                .GreaterThanOrEqualTo(1).WithMessage("CategoryId is not valid!");
            RuleFor(x => x.TagIds)
                .Must(CheckValidIds).WithMessage("TagIds is not valid");
            RuleFor(x => x.ColorIds)
                .Must(CheckValidIds).WithMessage("ColorIds is not valid");
        }

        private bool CheckValidIds(IEnumerable<int>? idsArr)
        {
            if (idsArr is null) return true;
           foreach (int id in idsArr)
            {
                if (id <= 0) return false;
            }
            
            return true;
        }
        
        //private async Task<bool> _isExist(string name, CancellationToken token)
        //{
        //    bool res = await _repository.IsExistEntityAsync(p => p.Name.ToLower() == name.ToLower());
        //    return res;
        //}
    }
}
