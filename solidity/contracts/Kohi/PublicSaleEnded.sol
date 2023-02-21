// SPDX-License-Identifier: MIT
/* Copyright (c) Kohi Art Community, Inc. All rights reserved. */

/*
/*
///////////////////////////////////////////////////////////////////////////////////
//                                                                               //
//     @@@@@@@@@@@@@@                        @@@@                                // 
//               @@@@                        @@@@ @@@@@@@@                       // 
//               @@@@    @@@@@@@@@@@@@@@@    @@@@@@@          @@@@@@@@@@@@@@@@   // 
//               @@@@                        @@@@                                // 
//     @@@@@@@@@@@@@@                        @@@@@@@@@@@@@                       // 
//               @@@@                          @@@@@@@@@@@                       // 
//                                                                               //
///////////////////////////////////////////////////////////////////////////////////
*/

pragma solidity ^0.8.13;

import "./IPublicMintable.sol";
import "@openzeppelin/contracts/access/Ownable.sol";
import "@openzeppelin/contracts/utils/Address.sol";

contract PublicSaleEnded is Ownable {
    using Address for address payable;

    IPublicMintable immutable parent;

    constructor(IPublicMintable _parent) {
        parent = _parent;
    }

    function publicMint(uint256 n) external payable {
        require(n != 0);
        revert("Public sale has ended.");
    }    
}