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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSLibrary
{
    public partial class HighLevelInterface
    {
        //byte[] _recvBuffer = new byte[8 + 255 + 20]; // receive packet buffer
        //int _currentRecvBufferSize = 0;
        //byte[] _recvBufferBackup = new byte[8 + 255 + 20]; // backup receive packet buffer
        //int _currentRecvBufferSizeBackup = 0;

        private void CharacteristicOnValueUpdated(byte [] data)
        {
            if (!CheckAPIHeader(data) || data[2] != (data.Length - 8))
                return;

            UInt16 recvCRC = (UInt16)(data[6] << 8 | data[7]);
            if (recvCRC != Tools.Crc.ComputeChecksum(data))
                return;

            ProcessAPIPacket(data);
            return;
        }

        byte _blePacketRunningNumber = 0x82;

        bool CheckAPIHeader(byte[] data)
        {
            return (data[0] == 0xa7 &&
                    data[1] == 0xb3 &&
                    // data[2] <= 120 && ignore Check Data Length
                    // data[4] == 0x82 &&
                    data[5] == 0x9e &&
                    (data[3] == 0xc2 ||
                     data[3] == 0x6a ||
                     data[3] == 0xd9 ||
                     data[3] == 0xe8 ||
                     data[3] == 0x5f)
                    );
        }
    }
}