#region File Description
//-----------------------------------------------------------------------------
// PlayerData.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Net;
#endregion

namespace BattleLines
{
    /// <summary>
    /// Data for each player in a network session.
    /// </summary>
    public class PlayerData
    {
        #region Gameplay Data

        #endregion


        #region Initialization Methods


        /// <summary>
        /// Constructs a PlayerData object.
        /// </summary>
        public PlayerData()
        {
            
        }


        #endregion


        #region Networking Methods


        /// <summary>
        /// Deserialize from the packet into the current object.
        /// </summary>
        /// <param name="packetReader">The packet reader that has the data.</param>
        public void Deserialize(PacketReader packetReader)
        {
            // safety-check the parameter, as it must be valid.
            if (packetReader == null)
            {
                throw new ArgumentNullException("packetReader");
            }

            //ShipColor = packetReader.ReadByte();
            //ShipVariation = packetReader.ReadByte();
        }


        /// <summary>
        /// Serialize the current object to a packet.
        /// </summary>
        /// <param name="packetWriter">The packet writer that receives the data.</param>
        public void Serialize(PacketWriter packetWriter)
        {
            // safety-check the parameter, as it must be valid.
            if (packetWriter == null)
            {
                throw new ArgumentNullException("packetWriter");
            }

            //packetWriter.Write(ShipColor);
            //packetWriter.Write(ShipVariation);
        }


        #endregion
    }
}
