// SPDX-License-Identifier: MIT
pragma solidity ^0.8.18;

import "@openzeppelin/contracts/access/Ownable.sol";
import "@openzeppelin/contracts/security/ReentrancyGuard.sol";
import "@openzeppelin/contracts/token/ERC20/IERC20.sol";

interface ICrowns is IERC20 {
    function burn(address account, uint256 amount) external;
    function mint(address to, uint256 amount) external;
    function gameTransfer(address from, address to, uint256 amount) external returns (bool);
}

// @title Pixel game for Warlord
// @author VersacePython

contract PixelGame is Ownable{
    uint256 private constant MAX_WIDTH = 960;
    uint256 private constant MAX_HEIGHT = 540;
    bytes32 public winningPixelHash;
    uint256 public iter;
    uint256 public currentRound;
    uint256 public guessMadeInRound;
    uint256 public reward = 1000 ether;

    //temp
    uint256 public _x;
    uint256 public _y;

    /// Betting token.
    ICrowns public crowns;

    struct Pixel {
        uint256 x;
        uint256 y;
        bool usedTreasureMap;
    }

    event pixelGuessed(address indexed player, uint256 indexed xcoord, uint256 indexed ycoord, uint256 round);

    Pixel[] private pixelArray;
    mapping(uint256 roundNumber => Pixel[]) public allGuessesInRound;
    mapping(uint256 roundNumber => mapping(address player => Pixel[])) public userGuesses;

    constructor(address _crowns) {
        crowns = ICrowns(_crowns);
        iter = 69;
        _generateNewWinningPixel();
    }

    function _generateNewWinningPixel() private {
        uint256 x = uint256(keccak256(abi.encodePacked(block.timestamp, iter))) % MAX_WIDTH + 1;
        uint256 y = uint256(keccak256(abi.encodePacked(block.timestamp, iter))) % MAX_HEIGHT + 1;
        winningPixelHash = hashPixelPair(x, y);
        _x = x;
        _y = y;
    }

    function hashPixelPair(uint256 x, uint256 y) public view returns(bytes32 hashedPair) {
        uint256 variable = iter - (iter / 2);
        bytes32 pairHash = keccak256(abi.encodePacked(x, y, variable));
        return pairHash;
    }

    function guessPixel(uint256 x, uint256 y, bool usedTreasureMap) external {
        require(x > 0 && x <= MAX_WIDTH && y > 0 && y <= MAX_HEIGHT, "Pixel out of range");
        // Charge players 5 Crowns per guess
        crowns.gameTransfer(msg.sender, address(this), 5 ether);

        Pixel memory newGuess = Pixel(x, y, usedTreasureMap);
        allGuessesInRound[currentRound].push(newGuess);
        userGuesses[currentRound][msg.sender].push(newGuess);
        guessMadeInRound++;

        bytes32 guessHash = hashPixelPair(x, y);
        if (guessHash == winningPixelHash) {
            emit pixelGuessed(msg.sender, x, y, currentRound);
            iter++;
            currentRound++;
            _generateNewWinningPixel();
            crowns.gameTransfer(msg.sender, address(this), reward);
        }
    }

    function getAllGuesses(uint256[] calldata indexes) external view returns (Pixel[] memory) {
        Pixel[] memory pixels = new Pixel[](indexes.length);
        for (uint256 i = 0; i < indexes.length; i++) {
            if (indexes[i] < allGuessesInRound[currentRound].length) {
                pixels[i] = allGuessesInRound[currentRound][i];
            }
        }
        return pixels;
    }

    function getUserGuesses(address user) external view returns (Pixel[] memory) {
        return userGuesses[currentRound][user];
    }

    function fundContractBalance(uint256 depositAmount) external onlyOwner {
        // Transfer Crowns from caller to this contract.
        crowns.gameTransfer(msg.sender, address(this), depositAmount);
    }
}
