/*
Copyright (c) 2018 Convergence Systems Limited

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System;
using System.Reflection;
using System.Text;

namespace CSLibrary
{
    public partial class RFIDReader
    {
        internal enum SCSLRFIDCMD
        {
            SCSLRFIDStartSimpleInventory = 0x10A1,
            SCSLRFIDStartCompactInventory = 0x10A2,
            SCSLRFIDStartSelectInventory = 0x10A3,
            SCSLRFIDStartMBInventory = 0x10A4,
            SCSLRFIDStartSelectMBInventory = 0x10A5,
            SCSLRFIDStartSelectCompactInventory = 0x10A6,
            SCSLRFIDStopOperation = 0x10AE,
            SCSLRFIDReadMB = 0x10B1,
            SCSLRFIDWriteMB = 0x10B2,
            SCSLRFIDWriteMBAny = 0x10B3,
            SCSLRFIDBlockWriteMB = 0x10B5,
            SCSLRFIDBlockWriteMBAny = 0x10B6,
            SCSLRFIDLock = 0x10B7,
            SCSLRFIDKill = 0x10B8,
            SCSLClearTagCacheTable = 0x10D3,
            SCSLUploadTagCacheTableToHost = 0x10D4,
            SCSLRFIDRegisterReset = 0x10D5,
            SCSLEx10Reset = 0x10D6,
            SCSLRFIDCircuitReset = 0x10D7,
            SCSLReadRegister = 0x1471,
            SCSLWriteRegister = 0x9a06,
        }

        byte[] RfidCmdpack1(SCSLRFIDCMD cmd, byte[] payload)
        {
            byte[] datapacket = new byte[4 + payload.Length];

            datapacket[0] = 0x80;
            datapacket[1] = 0xb3;
            datapacket[2] = (byte)((int)cmd >> 8);
            datapacket[3] = (byte)cmd;

            if (payload.Length > 0)
                Array.Copy(payload, 0, datapacket, 4, payload.Length);

            return datapacket;
        }

        internal void RunShortOperation(SCSLRFIDCMD cmd)
        {
            byte[] payload = new byte[3];

            byte[] datapacket = RfidCmdpack1(cmd, payload);
            _deviceHandler.SendAsync(0, 0, DOWNLINKCMD.RFIDCMD, datapacket, HighLevelInterface.BTWAITCOMMANDRESPONSETYPE.BTAPIRESPONSE, (uint)CurrentOperation);
        }

        internal void RFIDStartSimpleInventory()
        {
            RunShortOperation(SCSLRFIDCMD.SCSLRFIDStartSimpleInventory);
        }

        internal void RFIDStartCompactInventory()
        {
            RunShortOperation(SCSLRFIDCMD.SCSLRFIDStartCompactInventory);
        }

        internal void RFIDStartSelectInventory()
        {
            RunShortOperation(SCSLRFIDCMD.SCSLRFIDStartSelectInventory);
        }

        internal void RFIDStartMBInventory()
        {
            int index = 0;

            if (m_rdr_opt_parms.TagRanging.multibanks > 0)
            {
                RFIDRegister.MultibankReadConfig.Set(0, true, (int)m_rdr_opt_parms.TagRanging.bank1, m_rdr_opt_parms.TagRanging.offset1, m_rdr_opt_parms.TagRanging.count1);
                if (m_rdr_opt_parms.TagRanging.multibanks > 1)
                    RFIDRegister.MultibankReadConfig.Set(1, true, (int)m_rdr_opt_parms.TagRanging.bank2, m_rdr_opt_parms.TagRanging.offset2, m_rdr_opt_parms.TagRanging.count2);
                if (m_rdr_opt_parms.TagRanging.multibanks > 2)
                    RFIDRegister.MultibankReadConfig.Set(2, true, (int)m_rdr_opt_parms.TagRanging.bank3, m_rdr_opt_parms.TagRanging.offset3, m_rdr_opt_parms.TagRanging.count3);
                index = (int)m_rdr_opt_parms.TagRanging.multibanks;
            }

            for (;index < 3; index ++)
                RFIDRegister.MultibankReadConfig.Enable((byte)index, false);

            RunShortOperation(SCSLRFIDCMD.SCSLRFIDStartMBInventory);
        }

        internal void RFIDStartSelectMBInventory()
        {
            RunShortOperation(SCSLRFIDCMD.SCSLRFIDStartSelectMBInventory);
        }

        internal void RFIDStartSelectCompactInventory()
        {
            RunShortOperation(SCSLRFIDCMD.SCSLRFIDStartSelectCompactInventory);
        }

        internal void RFIDStopOperation()
        {
            RunShortOperation(SCSLRFIDCMD.SCSLRFIDStopOperation);
        }

        internal void RFIDReadMB()
        {
            RunShortOperation(SCSLRFIDCMD.SCSLRFIDReadMB);
        }

        internal void RFIDWriteMB()
        {
            RunShortOperation(SCSLRFIDCMD.SCSLRFIDWriteMB);
        }

        internal void RFIDWriteMBAny()
        {
            RunShortOperation(SCSLRFIDCMD.SCSLRFIDWriteMBAny);
        }

        internal void RFIDBlockWriteMB()
        {
            RunShortOperation(SCSLRFIDCMD.SCSLRFIDBlockWriteMB);
        }

        internal void RFIDBlockWriteMBAny()
        {
            RunShortOperation(SCSLRFIDCMD.SCSLRFIDBlockWriteMBAny);
        }

        internal void RFIDLockTag()
        {
            RunShortOperation(SCSLRFIDCMD.SCSLRFIDLock);
        }

        internal void RFIDKillTag()
        {
            RunShortOperation(SCSLRFIDCMD.SCSLRFIDKill);
        }

        internal void RFIDClearTagCacheTable()
        {
            RunShortOperation(SCSLRFIDCMD.SCSLClearTagCacheTable);
        }

        internal void RFIDUploadTagCacheTableToHost()
        {
            RunShortOperation(SCSLRFIDCMD.SCSLUploadTagCacheTableToHost);
        }

        internal void RFIDRegisterReset()
        {
            RunShortOperation(SCSLRFIDCMD.SCSLRFIDRegisterReset);
        }

        internal void RFIDEx10Reset()
        {
            RunShortOperation(SCSLRFIDCMD.SCSLEx10Reset);
        }

        internal void RFIDCircuitReset()
        {
            RunShortOperation(SCSLRFIDCMD.SCSLRFIDCircuitReset);
        }

        internal void RFIDReadRegister()
        {
            RunShortOperation(SCSLRFIDCMD.SCSLReadRegister);
        }

        internal void RFIDWriteRegister()
        {
            RunShortOperation(SCSLRFIDCMD.SCSLWriteRegister);
        }
    }
}