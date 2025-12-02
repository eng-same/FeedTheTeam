using Feed.Application.Services;
using Feed.Domain.Interfaces;
using Feed.Domain.Models;
using Moq;
using System.Threading.Tasks;
using Xunit;
namespace Feed.Tests;
public class VoteServiceTests
{
    private readonly Mock<IPoolRepository> _poolRepo;
    private readonly Mock<IPoolOptionsRepository> _optionRepo;
    private readonly Mock<IVotesRepository> _voteRepo;
    private readonly VoteService _service;

    public VoteServiceTests()
    {
        _poolRepo = new Mock<IPoolRepository>();
        _optionRepo = new Mock<IPoolOptionsRepository>();
        _voteRepo = new Mock<IVotesRepository>();

        _service = new VoteService(
            _voteRepo.Object ,       
            _poolRepo.Object,     
            _optionRepo.Object     
            );

    }

    [Fact]
    public async Task AddVoteAsync_ShouldCreateVote_WhenValid()
    {
        // Arrange
        _poolRepo
            .Setup(r => r.GetPoolByIdAsync(1))
            .ReturnsAsync(new Pool { Id = 1, Status = 0 });

        _optionRepo
            .Setup(r => r.GetPoolOptionByIdAsync(10))
            .ReturnsAsync(new PoolOption { Id = 10, PoolId = 1 });

        _voteRepo
            .Setup(r => r.HasUserVotedAsync(1, "user123"))
            .ReturnsAsync(false);

        _voteRepo
            .Setup(r => r.AddVoteAsync(It.IsAny<Vote>()))
            .Callback<Vote>(v => v.Id = 999)   
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.AddVoteAsync(1, 10, "user123");

        // Assert
        Assert.Equal(999, result.Id);
        Assert.Equal(1, result.PoolId);
        Assert.Equal(10, result.PoolOptionId);
        Assert.Equal("user123", result.UserId);

        _voteRepo.Verify(r => r.AddVoteAsync(It.IsAny<Vote>()), Times.Once);
    }
}
