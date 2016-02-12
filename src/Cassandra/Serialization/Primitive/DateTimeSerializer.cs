﻿//
//      Copyright (C) 2012-2016 DataStax Inc.
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.
//
using System;

namespace Cassandra.Serialization.Primitive
{
    internal class DateTimeSerializer : TypeSerializer<DateTime>
    {
        public override ColumnTypeCode CqlType
        {
            get { return ColumnTypeCode.Timestamp; }
        }

        public override DateTime Deserialize(ushort protocolVersion, byte[] buffer, IColumnInfo typeInfo)
        {
            var dto = DateTimeOffsetSerializer.Deserialize(buffer);
            return dto.DateTime;
        }

        public override byte[] Serialize(ushort protocolVersion, DateTime value)
        {
            // Treat "Unspecified" as UTC (+0) not the default behavior of DateTimeOffset which treats as Local Timezone
            // because we are about to do math against EPOCH which must align with UTC.
            var dateTimeOffset = value.Kind == DateTimeKind.Unspecified
                ? new DateTimeOffset(value, TimeSpan.Zero)
                : new DateTimeOffset(value);
            return DateTimeOffsetSerializer.Serialize(dateTimeOffset);
        }
    }
}