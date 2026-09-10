using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommBank.Controllers;
using CommBank.Models;
using CommBank.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CommBank.Tests;

public class GoalControllerTests
{
    private readonly Mock<IGoalService> _mockGoalService;
    private readonly GoalController _controller;

    public GoalControllerTests()
    {
        _mockGoalService = new Mock<IGoalService>();
        _controller = new GoalController(_mockGoalService.Object);
    }

    [Fact]
    public async Task GetGoalsForUser_ReturnsOkResult_WithListOfGoals()
    {
        // Arrange
        var userId = "test-user-id";
        var expectedGoals = new List<Goal>
        {
            new Goal
            {
                Id = "goal-1",
                UserId = userId,
                Name = "Holiday Fund",
                TargetAmount = 5000,
                Balance = 1200,
                Icon = "✈️"
            },
            new Goal
            {
                Id = "goal-2",
                UserId = userId,
                Name = "New Car",
                TargetAmount = 25000,
                Balance = 6000,
                Icon = "🚗"
            }
        };

        _mockGoalService
            .Setup(service => service.GetGoalsForUserAsync(userId))
            .ReturnsAsync(expectedGoals);

        // Act
        var result = await _controller.GetGoalsForUser(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedGoals = Assert.IsAssignableFrom<IEnumerable<Goal>>(okResult.Value);
        Assert.Equal(expectedGoals, returnedGoals);
    }

    [Fact]
    public async Task GetGoalsForUser_ReturnsNotFound_WhenNoGoalsExist()
    {
        // Arrange
        var userId = "empty-user-id";
        _mockGoalService
            .Setup(service => service.GetGoalsForUserAsync(userId))
            .ReturnsAsync(new List<Goal>());

        // Act
        var result = await _controller.GetGoalsForUser(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedGoals = Assert.IsAssignableFrom<IEnumerable<Goal>>(okResult.Value);
        Assert.Empty(returnedGoals);
    }
}