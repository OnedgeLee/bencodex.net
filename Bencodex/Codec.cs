using System;
using System.Diagnostics.Contracts;
using System.IO;
using Bencodex.Types;

namespace Bencodex
{
    /// <summary>The most basic and the lowest-level interface to encode and
    /// decode Bencodex values.  This provides two types of input and output:
    /// <c cref="byte">Byte</c> arrays and I/O
    /// <c cref="System.IO.Stream">Stream</c>s.</summary>
    public class Codec
    {
        /// <summary>
        /// Encodes a <paramref name="value"/> into a single <see cref="byte"/> array, rather than
        /// split into multiple chunks.</summary>
        /// <param name="value">A value to encode.</param>
        /// <param name="offloadOptions">Optionally configures how to offload heavy values included
        /// in lists and dictionaries.</param>
        /// <returns>A single <see cref="byte"/> array which contains the whole Bencodex
        /// representation of the <paramref name="value"/>.</returns>
        [Pure]
        public byte[] Encode(IValue value, IOffloadOptions? offloadOptions) =>
            Encoder.Encode(value, offloadOptions);

        /// <summary>Encodes a <paramref name="value"/>, and writes it on
        /// the <paramref name="output"/> stream.</summary>
        /// <param name="value">A value to encode.</param>
        /// <param name="output">A stream that a value is printed on.</param>
        /// <param name="offloadOptions">Optionally configures how to offload heavy values included
        /// in lists and dictionaries.</param>
        /// <exception cref="ArgumentException">Thrown when the given <paramref name="output"/>
        /// stream is not writable.</exception>
        public void Encode(IValue value, Stream output, IOffloadOptions? offloadOptions) =>
            Encoder.Encode(value, output, offloadOptions);

        /// <summary>
        /// Encodes a <paramref name="value"/> into a single
        /// <c cref="byte">Byte</c> array, rather than split into
        /// multiple chunks.</summary>
        /// <param name="value">A value to encode.</param>
        /// <returns>A single <c cref="byte">Byte</c> array which
        /// contains the whole Bencodex representation of
        /// the <paramref name="value"/>.</returns>
        [Pure]
        public byte[] Encode(IValue value) => Encoder.Encode(value, offloadOptions: null);

        /// <summary>Encodes a <paramref name="value"/>,
        /// and write it on an <paramref name="output"/> stream.</summary>
        /// <param name="value">A value to encode.</param>
        /// <param name="output">A stream that a value is printed on.</param>
        /// <exception cref="ArgumentException">Thrown when a given
        /// <paramref name="output"/> stream is not writable.</exception>
        public void Encode(IValue value, Stream output) =>
            Encode(value, output, offloadOptions: null);

        /// <summary>Decodes an encoded value with extended format for offloaded values from
        /// an <paramref name="input"/> stream.</summary>
        /// <param name="input">An input stream to decode.</param>
        /// <param name="indirectValueLoader">An optional <see cref="IndirectValue.Loader"/>
        /// delegate invoked when offloaded values are needed.</param>
        /// <returns>A decoded value.</returns>
        /// <exception cref="ArgumentException">Thrown when a given
        /// <paramref name="input"/> stream is not readable.</exception>
        /// <exception cref="DecodingException">Thrown when a binary representation of
        /// an <paramref name="input"/> stream is not a valid Bencodex encoding.</exception>
        public IValue Decode(Stream input, IndirectValue.Loader? indirectValueLoader)
        {
            if (!input.CanRead)
            {
                throw new ArgumentException("The input stream cannot be read.", nameof(input));
            }

            return new Decoder(input, indirectValueLoader).Decode();
        }

        /// <summary>Decodes an encoded value with extended format for offloaded values from
        /// a <see cref="byte"/> array.</summary>
        /// <param name="bytes">A <see cref="byte"/> array of Bencodex encoding.</param>
        /// <param name="indirectValueLoader">An optional <see cref="IndirectValue.Loader"/>
        /// delegate invoked when offloaded values are needed.</param>
        /// <returns>A decoded value.</returns>
        /// <exception cref="DecodingException">Thrown when a <paramref name="bytes"/>
        /// representation is not a valid Bencodex encoding.</exception>
        [Pure]
        public IValue Decode(byte[] bytes, IndirectValue.Loader? indirectValueLoader) =>
            Decode(new MemoryStream(bytes, false), indirectValueLoader);

        /// <summary>Decodes an encoded value from an <paramref name="input"/>
        /// stream.</summary>
        /// <param name="input">An input stream to decode.</param>
        /// <returns>A decoded value.</returns>
        /// <exception cref="ArgumentException">Thrown when a given
        /// <paramref name="input"/> stream is not readable.</exception>
        /// <exception cref="DecodingException">Thrown when a binary
        /// representation of an <paramref name="input"/> stream is not a valid
        /// Bencodex encoding.</exception>
        public IValue Decode(Stream input) => Decode(input, indirectValueLoader: null);

        /// <summary>Decodes an encoded value from a
        /// <c cref="byte">Byte</c> array.</summary>
        /// <param name="bytes">A <c cref="byte">Byte</c> array of
        /// Bencodex encoding.</param>
        /// <returns>A decoded value.</returns>
        /// <exception cref="DecodingException">Thrown when a
        /// <paramref name="bytes"/> representation is not a valid Bencodex
        /// encoding.</exception>
        [Pure]
        public IValue Decode(byte[] bytes) => Decode(bytes, indirectValueLoader: null);
    }
}
