﻿// Copyright (C) 2024 Maxim [maxirmx] Samsonov (www.sw.consulting)
// All rights reserved.
// This file is a part of dkg service node
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions
// are met:
// 1. Redistributions of source code must retain the above copyright
// notice, this list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright
// notice, this list of conditions and the following disclaimer in the
// documentation and/or other materials provided with the distribution.
//
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
// ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
// TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
// PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDERS OR CONTRIBUTORS
// BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
// CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
// SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
// INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
// CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
// ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
// POSSIBILITY OF SUCH DAMAGE.

using dkgServiceNode.Services.RoundRunner;
using System.ComponentModel.DataAnnotations.Schema;

namespace dkgServiceNode.Models
{

    [Table("rounds")]
    public class Round
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("status")]
        public short StatusValue { get; set; } = 0;

        [Column("node_count")]
        public int NodeCount { get; set; } = 0;

        [NotMapped]
        public int NodeCountRunning { get; set; } = 0;

        [NotMapped]
        public int NodeCountFinished { get; set; } = 0;

        [NotMapped]
        public int NodeCountFailed { get; set; } = 0;

        [Column("result")]
        public int? Result { get; set; } = null;

        [Column("created")]
        public DateTime CreatedOn { get; set; } = DateTime.Now.ToUniversalTime();

        [Column("modified")]
        public DateTime ModifiedOn { get; set; } = DateTime.Now.ToUniversalTime();

        [NotMapped]
        public RoundStatus Status
        {
            get { return RoundStatusConstants.GetRoundStatusById(StatusValue); }
            set { StatusValue = (short)value.RoundStatusId; }
        }

        [NotMapped]
        public bool IsVersatile
        {
            get { return Status.IsVersatile(); }
        }

        [NotMapped]
        public RoundStatus NextStatus
        {
            get { return Status.NextStatus(); }
        }
        [NotMapped]
        public RoundStatus CancelStatus
        {
            get { return Status.CancelStatus(); }
        }

        public ICollection<Node> Nodes { get; set; } = [];
    }
}
