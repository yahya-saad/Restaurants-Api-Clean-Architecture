using FluentValidation.TestHelper;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Tests;
public class CreateRestaurantCommandValidatorTests
{
    private CreateRestaurantDto GetValidDto()
    {
        return new CreateRestaurantDto
        {
            Name = "Test Restaurant",
            Description = "Test Description",
            Category = "Fast Food",
            HasDelivery = true,
            ContractEmail = null,
            ContractNumber = null,
            Address = new Address
            {
                Street = "123 Test St",
                City = "Test City",
                PostalCode = "12345"
            }
        };
    }

    [Fact]
    public void Validate_WithValidCommand_ShouldNotHaveValidationErrors()
    {
        var dto = GetValidDto();
        var command = new CreateRestaurantCommand(dto);
        var validator = new CreateRestaurantCommandValidator();

        var result = validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Validate_WithInvalidCommand_ShouldHaveValidationErrors()
    {

        var dto = GetValidDto();
        dto.Name = "";
        dto.Description = "";
        dto.Category = "Invalid Category";
        dto.ContractEmail = "invalid-email";
        dto.ContractNumber = "invalid-number";
        dto.Address!.PostalCode = "invalid-postal-code";


        var command = new CreateRestaurantCommand(dto);
        var validator = new CreateRestaurantCommandValidator();
        var result = validator.TestValidate(command);

        result.ShouldHaveValidationErrors();
        result.ShouldHaveValidationErrorFor(x => x.Dto.Name);
        result.ShouldHaveValidationErrorFor(x => x.Dto.Description);
        result.ShouldHaveValidationErrorFor(x => x.Dto.Category);
        result.ShouldHaveValidationErrorFor(x => x.Dto.ContractEmail);
        result.ShouldHaveValidationErrorFor(x => x.Dto.ContractNumber);
        result.ShouldHaveValidationErrorFor(x => x.Dto.Address!.PostalCode);
    }

    [Fact]
    public void Validate_WithEmptyName_ShouldHaveValidationErrorForNameProperty()
    {
        var dto = GetValidDto();
        dto.Name = "";
        var command = new CreateRestaurantCommand(dto);
        var validator = new CreateRestaurantCommandValidator();
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Dto.Name);

    }

    [Fact]
    public void Validate_WithTooShortName_ShouldHaveValidationErrorForNameProperty()
    {
        var dto = GetValidDto();
        dto.Name = "no";
        var command = new CreateRestaurantCommand(dto);
        var validator = new CreateRestaurantCommandValidator();
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Dto.Name);
    }

    [Fact]
    public void Validate_WithTooLongName_ShouldHaveValidationErrorForNameProperty()
    {
        var dto = GetValidDto();
        dto.Name = new string('a', 101);

        var command = new CreateRestaurantCommand(dto);
        var validator = new CreateRestaurantCommandValidator();
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Dto.Name);
    }

    [Fact]
    public void Validate_WithEmptyDescription_ShouldHaveValidationErrorForDescriptionProperty()
    {
        var dto = GetValidDto();
        dto.Description = "";
        var command = new CreateRestaurantCommand(dto);
        var validator = new CreateRestaurantCommandValidator();
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Dto.Description);
    }

    [Fact]
    public void Validate_WithTooLongDescription_ShouldHaveValidationErrorForDescriptionProperty()
    {
        var dto = GetValidDto();
        dto.Description = new string('a', 501);
        var command = new CreateRestaurantCommand(dto);
        var validator = new CreateRestaurantCommandValidator();
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Dto.Description);
    }

    [Theory]
    [InlineData("Fast Food")]
    [InlineData("Egyptian")]
    [InlineData("Italian")]
    [InlineData("Chinese")]
    [InlineData("Japanese")]

    public void Validate_WithValidCategory_ShouldNotHaveValidationErrorForCategoryProperty(string expected)
    {
        var dto = GetValidDto();
        dto.Category = expected;
        var command = new CreateRestaurantCommand(dto);
        var validator = new CreateRestaurantCommandValidator();
        var result = validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Dto.Category);
    }

    [Fact]
    public void Validate_WithInvalidCategory_ShouldHaveValidationErrorForCategoryProperty()
    {
        var dto = GetValidDto();
        dto.Category = "Invalid Category";
        var command = new CreateRestaurantCommand(dto);
        var validator = new CreateRestaurantCommandValidator();
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Dto.Category);
    }

    [Theory]
    [InlineData("test@example.com")]
    [InlineData("user.name+tag+sorting@example.com")]
    [InlineData("x@x.au")]
    [InlineData("user_name@domain.co")]
    public void Validate_WithValidEmail_ShouldNotHaveValidationErrorForEmailProperty(string email)
    {
        var dto = GetValidDto();
        dto.ContractEmail = email;
        var command = new CreateRestaurantCommand(dto);
        var validator = new CreateRestaurantCommandValidator();
        var result = validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Dto.ContractEmail);
    }

    [Theory]
    [InlineData("plainaddress")]
    [InlineData("missingatsign.com")]
    [InlineData("missingdomain@")]
    [InlineData("@missingusername.com")]
    public void Validate_WithInvalidEmail_ShouldHaveValidationErrorForEmailProperty(string expected)
    {
        var dto = GetValidDto();
        dto.ContractEmail = expected;
        var command = new CreateRestaurantCommand(dto);
        var validator = new CreateRestaurantCommandValidator();
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Dto.ContractEmail);
    }

    [Theory]
    [InlineData("01012345678")]
    [InlineData("01123456789")]
    [InlineData("01298765432")]
    [InlineData("01511111111")]
    [InlineData("+201012345678")]
    [InlineData("+201123456789")]
    public void Validate_WithValidPhoneNumber_ShouldNotHaveValidationErrorForPhoneNumberProperty(string number)
    {
        var dto = GetValidDto();
        dto.ContractNumber = number;
        var command = new CreateRestaurantCommand(dto);
        var validator = new CreateRestaurantCommandValidator();
        var result = validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Dto.ContractNumber);
    }


    [Theory]
    [InlineData("1234567890")]       // doesn't start with 01 or +20
    [InlineData("01712345678")]      // 7 is not a valid second digit
    [InlineData("0112345678")]       // too short
    [InlineData("012345678901")]     // too long
    [InlineData("01A23456789")]      // contains letter
    [InlineData("011@3456789")]      // contains special char
    [InlineData("+201712345678")]    // 7 is not a valid second digit
    [InlineData("00123456789")]      // invalid country prefix
    public void Validate_WithInvalidPhoneNumber_ShouldHaveValidationErrorForPhoneNumberProperty(string number)
    {
        var dto = GetValidDto();
        dto.ContractNumber = number;
        var command = new CreateRestaurantCommand(dto);
        var validator = new CreateRestaurantCommandValidator();
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Dto.ContractNumber);
    }

    [Theory]
    [InlineData("12345")]
    [InlineData("00000")]
    [InlineData("99999")]
    public void Validate_WithValidPostalCode_ShouldNotHaveValidationErrorForPostalCodeProperty(string validPostalCode)
    {
        var dto = GetValidDto();
        dto.Address!.PostalCode = validPostalCode;
        var command = new CreateRestaurantCommand(dto);
        var validator = new CreateRestaurantCommandValidator();
        var result = validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Dto.Address.PostalCode);
    }


    [Theory]
    [InlineData("abcde")]          // letters only
    [InlineData("1234")]           // too short
    [InlineData("123456")]         // too long
    [InlineData("12 345")]         // spaces
    [InlineData("12-345")]         // special characters
    public void Validate_WithInvalidPostalCode_ShouldHaveValidationErrorForPostalCodeProperty(string invalidPostalCode)
    {
        var dto = GetValidDto();
        dto.Address!.PostalCode = invalidPostalCode;
        var command = new CreateRestaurantCommand(dto);
        var validator = new CreateRestaurantCommandValidator();
        var result = validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Dto.Address.PostalCode);
    }



}
