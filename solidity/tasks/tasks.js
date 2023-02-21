const fs = require('fs');
const quiet = false;
const GLYPH_DIR = "./font/";

task("deploy", "Deploy contract to network")
    .setAction(async (taskArgs, hre) => {
        await deployAll();
    });

async function deployAll() {
    let networkName = network.name;
    if (network.chainId == 8134646) {
        networkName = "mochi";
    } else if (network.chainId == 1337) {
        networkName = "hardhat";
    }

    console.log();
    console.log("Network:");
    console.log("------------------------------------------------------------------------");
    console.log(networkName);

    console.log();
    console.log("Kohi:");
    console.log("------------------------------------------------------------------------");
    let kohi = await deployKohi();

    console.log();
    console.log("the Universe Machine:");
    console.log("------------------------------------------------------------------------");
    let tum = await deployUniverseMachine(kohi);

    console.log();
    console.log("Manifest:");
    console.log("------------------------------------------------------------------------");
    let manifest = mergeJSON(kohi, tum);

    let output = outputManifest(manifest);
    console.log(output);

    fs.writeFileSync(`../UniverseMachine.Renderer/manifest-${networkName}.json`, JSON.stringify(output));    
}

async function deployKohi() {
    let manifest = {};
    await deployContract(manifest, "SinLut256");
    await deployContract(manifest, "EllipseMethods", {
        libraries: {
            SinLut256: manifest.SinLut256.address
        },
    });
    await deployContract(manifest, "CustomPathMethods");
    await deployContract(manifest, "StrokeMethods", {
        libraries: {
            SinLut256: manifest.SinLut256.address
        }
    });
    await deployContract(manifest, "AntiAliasMethods");
    await deployContract(manifest, "CellBlockMethods");
    await deployContract(manifest, "CellDataMethods", {
        libraries: {
            CellBlockMethods: manifest.CellBlockMethods.address
        }
    });
    await deployContract(manifest, "ClippingDataMethods");
    await deployContract(manifest, "ScanlineDataMethods");
    await deployContract(manifest, "SubpixelScaleMethods");
    await deployContract(manifest, "Graphics2DMethods", {
        libraries: {
            AntiAliasMethods: manifest.AntiAliasMethods.address,
            CellDataMethods: manifest.CellDataMethods.address,
            ClippingDataMethods: manifest.ClippingDataMethods.address,
            ScanlineDataMethods: manifest.ScanlineDataMethods.address,
            SubpixelScaleMethods: manifest.SubpixelScaleMethods.address,
        }
    });
    return manifest;
}

async function deployUniverseMachine(kohi) {
    let manifest = {};    
    await deployContract(manifest, "UniverseMachineGrid", {
        libraries: {
            EllipseMethods: kohi.EllipseMethods.address
        }
    });
    await deployContract(manifest, "UniverseMachineSkeletonFactory", {
        libraries: {
            EllipseMethods: kohi.EllipseMethods.address
        }
    });
    await deployContract(manifest, "UniverseMachineSkeleton");
    await deployContract(manifest, "Texture0Factory", {
        libraries: {
            EllipseMethods: kohi.EllipseMethods.address,
            StrokeMethods: kohi.StrokeMethods.address
        }
    });
    await deployContract(manifest, "Texture1Factory", {
        libraries: {
            CustomPathMethods: kohi.CustomPathMethods.address
        }
    });
    await deployContract(manifest, "Texture2Factory", {
        libraries: {
            CustomPathMethods: kohi.CustomPathMethods.address,
            StrokeMethods: kohi.StrokeMethods.address
        }
    });
    await deployContract(manifest, "Texture3Factory", {
        libraries: {
            EllipseMethods: kohi.EllipseMethods.address
        }
    });
    await deployContract(manifest, "Texture4Factory", {
        libraries: {
            CustomPathMethods: kohi.CustomPathMethods.address,
            EllipseMethods: kohi.EllipseMethods.address,
            StrokeMethods: kohi.StrokeMethods.address
        }
    });
    await deployContract(manifest, "Texture5Factory", {
        libraries: {
            EllipseMethods: kohi.EllipseMethods.address
        }
    });
    await deployContract(manifest, "UniverseMachineUniverseFactory", {
        libraries: {
            EllipseMethods: kohi.EllipseMethods.address,
            SinLut256: kohi.SinLut256.address
        }
    });
    await deployContract(manifest, "UniverseMachineUniverse");
    await deployContract(manifest, "UniverseMachineStarsFactory", {
        libraries: {
            SinLut256: kohi.SinLut256.address
        }
    });
    await deployContract(manifest, "UniverseMachineStars");
    await deployContract(manifest, "UniverseMachineMatsFactory", {
        libraries: {
            CustomPathMethods: kohi.CustomPathMethods.address,
            StrokeMethods: kohi.StrokeMethods.address
        }
    });
    await deployContract(manifest, "UniverseMachineMats");
    await deployContract(manifest, "UniverseMachineParameters");
    await deployContract(manifest, "UniverseMachineRenderer", {
        libraries: {
            Graphics2DMethods: kohi.Graphics2DMethods.address,
            Texture0Factory: manifest.Texture0Factory.address,
            Texture1Factory: manifest.Texture1Factory.address,
            Texture2Factory: manifest.Texture2Factory.address,
            Texture3Factory: manifest.Texture3Factory.address,
            Texture4Factory: manifest.Texture4Factory.address,
            Texture5Factory: manifest.Texture5Factory.address,
            UniverseMachineGrid: manifest.UniverseMachineGrid.address,
            UniverseMachineSkeletonFactory: manifest.UniverseMachineSkeletonFactory.address,
            UniverseMachineSkeleton: manifest.UniverseMachineSkeleton.address,
            UniverseMachineUniverseFactory: manifest.UniverseMachineUniverseFactory.address,
            UniverseMachineUniverse: manifest.UniverseMachineUniverse.address,
            UniverseMachineStars: manifest.UniverseMachineStars.address,
            UniverseMachineStarsFactory: manifest.UniverseMachineStarsFactory.address,
            UniverseMachineMats: manifest.UniverseMachineMats.address,
            UniverseMachineMatsFactory: manifest.UniverseMachineMatsFactory.address,
        }
    });

    return manifest;
}

async function deployContract(manifest, contractName, libraries) {
    let contract = {};
    if (libraries) {
        contract = await ethers.getContractFactory(contractName, libraries);
    } else {
        contract = await ethers.getContractFactory(contractName);
    }
    var deployed = await contract.deploy();
    await deployed.deployed();
    console.log(`${contractName} deployed to: ${deployed.address}`);
    manifest[contractName] = deployed;
}

function outputManifest(manifest) {
    var output = {};
    for (var property in manifest) {
        if (manifest[property].address) {
            output[property] = manifest[property].address;
        }
    }
    return output;
}

function mergeJSON(json1, json2) {
    var result = {};
    for (var prop in json1) {
        result[prop] = json1[prop];
    }
    for (var prop in json2) {
        result[prop] = json2[prop];
    }
    return result;
}