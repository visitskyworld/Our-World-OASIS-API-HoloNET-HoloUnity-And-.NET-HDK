﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NextGenSoftware.OASIS.API.Core.Enums;
using NextGenSoftware.OASIS.API.Core.Events;
using NextGenSoftware.OASIS.API.Core.Helpers;
using NextGenSoftware.OASIS.API.Core.Holons;
using NextGenSoftware.OASIS.API.Core.Interfaces;
using NextGenSoftware.OASIS.API.Core.Managers;
using NextGenSoftware.OASIS.API.DNA.Manager;

namespace NextGenSoftware.OASIS.STAR.Zomes
{
    //public abstract class ZomeBase: Holon, IZome
    public abstract class ZomeBase : Holon, IZomeBase
    {
        private HolonManager _holonManager = new HolonManager(OASISDNAManager.GetAndActivateDefaultProvider());
        public List<Holon> _holons = new List<Holon>();

        public List<Holon> Holons
        {
            get
            {
                return _holons;
            }
            set
            {
                _holons = value;
            }
        }

        public delegate void HolonSaved(object sender, HolonSavedEventArgs e);
        public event HolonSaved OnHolonSaved;

        public delegate void HolonLoaded(object sender, HolonLoadedEventArgs e);
        public event HolonLoaded OnHolonLoaded;

        public delegate void HolonsLoaded(object sender, HolonsLoadedEventArgs e);
        public event HolonsLoaded OnHolonsLoaded;

        public delegate void Initialized(object sender, EventArgs e);
        public event Initialized OnInitialized;

        public delegate void ZomeError(object sender, ZomeErrorEventArgs e);
        public event ZomeError OnZomeError;

        ////TODO: Not sure if we want to expose the HoloNETClient events at this level? They can subscribe to them through the HoloNETClient property below...
        //public delegate void Disconnected(object sender, DisconnectedEventArgs e);
        //public event Disconnected OnDisconnected;

        //public delegate void DataReceived(object sender, DataReceivedEventArgs e);
        //public event DataReceived OnDataReceived;

        public virtual async Task<IHolon> LoadHolonAsync(Guid id, HolonType type = HolonType.Holon)
        {
            return await _holonManager.LoadHolonAsync(id, type);
        }

        public virtual async Task<IHolon> LoadHolonAsync(Dictionary<ProviderType, string> providerKey, HolonType type = HolonType.Holon)
        {
            return await _holonManager.LoadHolonAsync(GetCurrentProviderKey(providerKey));
        }

        public virtual async Task<IEnumerable<IHolon>> LoadHolonsAsync(Guid id, HolonType type = HolonType.Holon)
        {
            return await _holonManager.LoadHolonsForParentAsync(id, type);
        }

        public virtual async Task<IEnumerable<IHolon>> LoadHolonsAsync(Dictionary<ProviderType, string> providerKey, HolonType type = HolonType.Holon)
        {
            return await _holonManager.LoadHolonsForParentAsync(GetCurrentProviderKey(providerKey), type);
        }

        public virtual async Task<OASISResult<IHolon>> SaveHolonAsync(IHolon savingHolon)
        {
            return await _holonManager.SaveHolonAsync(savingHolon);
        }

        public virtual async Task<OASISResult<IEnumerable<IHolon>>> SaveHolonsAsync(IEnumerable<IHolon> savingHolons)
        {
            return await _holonManager.SaveHolonsAsync(savingHolons);
        }

        public virtual async Task<OASISResult<IEnumerable<IHolon>>> Save()
        {
            return await _holonManager.SaveHolonsAsync(this.Holons);
        }

        public async Task<OASISResult<IEnumerable<IHolon>>> AddHolon(IHolon holon)
        {
            //return await base.SaveHolonAsync(string.Concat(this.Name, HOLONS_ADD), zome);
            this.Holons.Add((Holon)holon);
            return await SaveHolonsAsync(this.Holons);
        }

        public async Task<OASISResult<IEnumerable<IHolon>>> RemoveHolon(IHolon holon)
        {
            //return await base.SaveHolonAsync(string.Concat(this.Name, HOLONS_REMOVE), zome);
            this.Holons.Remove((Holon)holon);
            return await SaveHolonsAsync(this.Holons);
        }

        private string GetCurrentProviderKey(Dictionary<ProviderType, string> providerKey)
        {
            if (ProviderKey.ContainsKey(ProviderManager.CurrentStorageProviderType.Value) && !string.IsNullOrEmpty(ProviderKey[ProviderManager.CurrentStorageProviderType.Value]))
                return providerKey[ProviderManager.CurrentStorageProviderType.Value];
            else
                throw new Exception(string.Concat("ProviderKey not found for CurrentStorageProviderType ", ProviderManager.CurrentStorageProviderType.Name));

            //TODO: Return OASISResult instead of throwing exceptions for ALL OASIS methods!
        }
    }
}