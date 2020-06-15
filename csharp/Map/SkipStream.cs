// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Map
{
    public struct SkipRegion
    {
        public long Start;
        public long Length;
    }

    public class SkipStream : Stream
    {
        private Stream _source;
        private List<SkipRegion> _skipRegions;
        private byte[] _skipReplacement;

        private long _adjustedPosition;
        private long _adjustedLength;

        private int _nextSkipRegionIndex;
        private int _replacementBytesPending;

        public override bool CanRead => true;
        public override bool CanSeek => true;
        public override bool CanWrite => true;
        public override long Length => _adjustedLength;

        public override long Position
        {
            get => _adjustedPosition;
            set { Seek(value, SeekOrigin.Begin); }
        }

        public SkipStream(Stream source, IEnumerable<SkipRegion> skipRegions, byte[] skipReplacement) : base()
        {
            _source = source;
            _skipRegions = skipRegions.OrderBy(sr => sr.Start).ToList();
            _skipReplacement = skipReplacement;

            // Length of filtered stream excludes all skipped regions but adds a replacement for each
            _adjustedLength = source.Length - _skipRegions.Sum(sr => sr.Length) + _skipRegions.Count * skipReplacement.Length;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();

            //// Convert offset to be an absolute position in the transformed version of the stream
            //long adjustedPosition = ToAdjustedAbsoluteOffset(offset, origin);

            //// Convert position to a position in the real underlying stream
            //long underlyingPosition = adjustedPosition;

            //int regionIndex;
            //for (regionIndex = 0; regionIndex < _skipRegions.Count; ++regionIndex)
            //{
            //    // If this region is after the desired location, we're done accounting for SkipRegions
            //    if (adjustedPosition < _skipRegions[regionIndex].Start) { break; }

            //    // Otherwise, we will skip after this Region in the underlying file
            //    underlyingPosition += _skipRegions[regionIndex].Length - _replacementBytesPending;
            //}
        }

        /// <summary>
        ///  
        /// </summary>
        private long ToAdjustedAbsoluteOffset(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    return offset;

                case SeekOrigin.Current:
                    return _adjustedPosition + offset;

                case SeekOrigin.End:
                    return _adjustedLength + offset;

                default:
                    throw new NotImplementedException($"SkipStream.ToStartRelativeOffset not implemented for SeekOrigin {origin}");
            }
        }

        public override void Flush()
        {
            // Not writeable, so nothing to do
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException("SkipStream does not support writing.");
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotSupportedException("SkipStream does not support writing.");
        }
    }
}
