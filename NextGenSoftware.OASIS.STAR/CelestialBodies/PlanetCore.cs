﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NextGenSoftware.OASIS.API.Core.Enums;
using NextGenSoftware.OASIS.API.Core.Helpers;
using NextGenSoftware.OASIS.API.Core.Interfaces;
using NextGenSoftware.OASIS.API.Core.Interfaces.STAR;

namespace NextGenSoftware.OASIS.STAR.CelestialBodies
{
    public class PlanetCore : CelestialBodyCore, IPlanetCore
    {
        public IPlanet Planet { get; set; }

        public PlanetCore(IPlanet planet) : base()
        {
            this.Planet = planet;
        }

        public PlanetCore(IPlanet planet, Dictionary<ProviderType, string> providerKey) : base(providerKey)
        {
            this.Planet = planet;
        }

        public PlanetCore(IPlanet planet, Guid id) : base(id)
        {
            this.Planet = planet;
        }

        //public async Task<OASISResult<IMoon>> AddMoonAsync(IMoon moon)
        //{
        //    return OASISResultHolonToHolonHelper<IHolon, IMoon>.CopyResult(
        //        await AddHolonToCollectionAsync(Planet, moon, (List<IHolon>)Mapper<IMoon, Holon>.MapBaseHolonProperties(
        //            Planet.Moons)), new OASISResult<IMoon>());
        //}

        //public OASISResult<IMoon> AddMoon(IMoon moon)
        //{
        //    return AddMoonAsync(moon).Result; //TODO: Is this the best way of doing this?
        //}

        public async Task<OASISResult<IEnumerable<IMoon>>> GetMoonsAsync(bool refresh = true, bool loadChildren = true, bool recursive = true, int maxChildDepth = 0, bool continueOnError = true, int version = 0)
        {
            OASISResult<IEnumerable<IMoon>> result = new OASISResult<IEnumerable<IMoon>>();
            OASISResult<IEnumerable<IHolon>> holonResult = await GetHolonsAsync(Planet.Moons, HolonType.Moon, refresh, loadChildren, recursive, maxChildDepth, continueOnError, version);
            OASISResultCollectionToCollectionHelper<IEnumerable<IHolon>, IEnumerable<IMoon>>.CopyResult(holonResult, ref result);
            result.Result = Mapper<IHolon, Moon>.MapBaseHolonProperties(holonResult.Result);
            return result;
        }

        public OASISResult<IEnumerable<IMoon>> GetMoons(bool refresh = true, bool loadChildren = true, bool recursive = true, int maxChildDepth = 0, bool continueOnError = true, int version = 0)
        {
            return GetMoonsAsync(refresh).Result; //TODO: Is this the best way of doing this?
        }

        public async Task<OASISResult<IEnumerable<IMoon>>> SaveMoonsAsync(bool saveChildren = true, bool recursive = true, int maxChildDepth = 0, bool continueOnError = true)
        {
            OASISResult<IEnumerable<IMoon>> result = new OASISResult<IEnumerable<IMoon>>();

            if (Planet.Moons == null)
                Planet.Moons = new List<IMoon>();

            if (Planet.Moons.Count == 0)
            {
                result.IsWarning = true;
                result.Message = "No moons found to save.";
                return result;
            }
            
            OASISResult<IEnumerable<IHolon>> holonResult = await SaveHolonsAsync(Planet.Moons, true, saveChildren, recursive, maxChildDepth, continueOnError);
            OASISResultCollectionToCollectionHelper<IEnumerable<IHolon>, IEnumerable<IMoon>>.CopyResult(holonResult, ref result);
            result.Result = Mapper<IHolon, Moon>.MapBaseHolonProperties(holonResult.Result);
            return result;
        }

        public OASISResult<IEnumerable<IMoon>> SaveMoons(bool saveChildren = true, bool recursive = true, int maxChildDepth = 0, bool continueOnError = true)
        {
            return SaveMoonsAsync(saveChildren, recursive, maxChildDepth, continueOnError).Result; //TODO: Is this the best way of doing this?
        }
    }
}
