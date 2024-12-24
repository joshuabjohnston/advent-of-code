using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using org.jjohnston.aoc.days;

namespace org.jjohnston.aoc.year2024;

public class Day9 : AbstractDay
{
    public enum DiskBlockType
    {
        Empty,
        File,
    }

    public class DiskBlock
    {
        public DiskBlockType Type { get; set; } = DiskBlockType.Empty;
        public int FileID { get; set; } = 0;

        public override string ToString()
        {
            return (this.Type == DiskBlockType.Empty) ? "." : this.FileID.ToString();
        }

        public static DiskBlock Empty()
        {
            return new DiskBlock() { Type = DiskBlockType.Empty, FileID = 0 };
        }

        public static DiskBlock File(int fileID)
        {
            return new DiskBlock { Type = DiskBlockType.File, FileID = fileID };
        }
    }

    public class DiskRegion : DiskBlock
    {
        public int Size { get; set; } = 0;
    }

    public class Disk
    {
        public List<DiskBlock> Blocks { get; set; } = new List<DiskBlock>();
        public List<DiskRegion> Regions { get; set; } = new List<DiskRegion>();

        public void AddEmptyBlock()
        {
            this.Blocks.Add(DiskBlock.Empty());
        }

        public void AddFileBlock(int fileID)
        {
            this.Blocks.Add(DiskBlock.File(fileID));
        }

        public void AddBlocksForFileOfSize(int fileSize, int fileID)
        {
            for (int s = 0; s < fileSize; s++)
            {
                this.AddFileBlock(fileID);
            }
        }

        public void AddBlocksForEmptyOfSize(int numEmpty)
        {
            for (int s = 0; s < numEmpty; s++)
            {
                this.AddEmptyBlock();
            }
        }

        public void AddEmptyRegion(int sz)
        {
            if (sz > 0)
            {
                this.Regions.Add(new DiskRegion() { Type = DiskBlockType.Empty, FileID = 0, Size = sz });
            }
        }

        public void AddFileRegion(int sz, int fileID)
        {
            if (sz > 0)
            {
                this.Regions.Add(new DiskRegion() { Type = DiskBlockType.File, FileID = fileID, Size = sz });
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (DiskBlock b in Blocks)
            {
                sb.Append(b.ToString());
            }

            return sb.ToString();
        }

        public string ToStringRegions()
        {
            StringBuilder sb = new StringBuilder();

            foreach (DiskRegion r in Regions)
            {
                for (int s = 0; s < r.Size; s++)
                {
                    sb.Append(r.ToString());
                }
                sb.Append("|");
            }

            return sb.ToString();
        }

        public void CompressByIndividualBlock()
        {
            // in from 0 finding blank spaces
            int beg = 0;
            // in from Length finding non-blank spaces
            int end = this.Blocks.Count() - 1;

            while (end > beg)
            {
                // are we looking at a file block at the end? and a blank spot to put it near the front?
                if (Blocks[end].Type == DiskBlockType.File)
                {
                    if (Blocks[beg].Type == DiskBlockType.Empty)
                    {
                        // swap this file into this blank position
                        DiskBlock temp = Blocks[end];
                        Blocks[end] = Blocks[beg];
                        Blocks[beg] = temp;

                        beg++;
                        end--;
                    }
                    else
                    {
                        beg++;
                    }
                }
                else
                {
                    end--;
                }
            }
        }

        public void MergeEmptyRegions()
        {
            for (int i = 0; i < this.Regions.Count() - 1; i++)
            {
                int j = i + 1;
                if (j >= Regions.Count()) break;
                if (Regions[i].Type == DiskBlockType.Empty && Regions[j].Type == DiskBlockType.Empty)
                {
                    int newSize = Regions[i].Size + Regions[j].Size;
                    DiskRegion newEmpty = new DiskRegion() { Type = DiskBlockType.Empty, FileID = 0, Size = newSize };
                    Regions[i] = newEmpty;
                    Regions.RemoveAt(j);
                    if (i > 0) --i;
                }
            }
        }

        public void CompressByRegion(bool debug = false)
        {
            MergeEmptyRegions();

            HashSet<int> consideredFiles = new HashSet<int>();

            // start from the END
            for (int end = this.Regions.Count() - 1; end > 0; end--)
            {
                DiskRegion regionAtEnd = this.Regions[end];
                if (regionAtEnd.Type == DiskBlockType.File
                    && !consideredFiles.Contains(regionAtEnd.FileID))
                {
                    if (debug) Console.Out.WriteLine($"considering {regionAtEnd.FileID}");
                    consideredFiles.Add(regionAtEnd.FileID);

                    // if this is a file, what's the first empty region it fits into, from the BEGINNING?
                    for (int beg = 0; beg < end; beg++)
                    {
                        if (end >= Regions.Count())
                        {
                            return;
                        }
                        regionAtEnd = this.Regions[end];
                        DiskRegion regionAtBeg = this.Regions[beg];

                        if (regionAtBeg.Type == DiskBlockType.Empty
                                && regionAtBeg.Size >= regionAtEnd.Size)
                        {
                            int fileSize = regionAtEnd.Size;
                            int fileID = regionAtEnd.FileID;
                            int emptySize = regionAtBeg.Size;

                            // the region at the end is now empty
                            regionAtEnd.Type = DiskBlockType.Empty;
                            regionAtEnd.FileID = 0;

                            // the region at the front is now a file.
                            regionAtBeg.Type = DiskBlockType.File;
                            regionAtBeg.Size = fileSize;
                            regionAtBeg.FileID = fileID;

                            // plus we need to add any leftover space as an empty region AFTER this file
                            if (fileSize < emptySize)
                            {
                                int leftoverEmptySpace = emptySize - fileSize;
                                DiskRegion newEmpty = new DiskRegion() { Type = DiskBlockType.Empty, FileID = 0, Size = leftoverEmptySpace };
                                this.Regions.Insert(beg + 1, newEmpty);
                            }
                            this.MergeEmptyRegions();

                            if (debug) Console.Out.WriteLine($" -- [step]    * {this.ToStringRegions()}");

                            end = Regions.Count() - 1;
                            break;
                        }
                    }
                }
            }
        }

        public long Checksum()
        {
            long cs = 0;

            for (int i = 0; i < this.Blocks.Count(); i++)
            {
                cs += i * Blocks[i].FileID;
            }

            return cs;
        }

        public long CheckSumByRegion()
        {
            long cs = 0;

            int blockNum = 0;

            for (int r = 0; r < this.Regions.Count(); r++)
            {
                DiskRegion thisReg = this.Regions[r];
                for (int b = 0; b < thisReg.Size; b++)
                {
                    if (thisReg.Type == DiskBlockType.File)
                    {
                        cs += thisReg.FileID * blockNum;
                    }
                    ++blockNum;
                }
            }

            return cs;
        }
    }

    public override string Star_1_Impl(string[] inputs, bool debug)
    {
        long checksum = 0;

        Disk disk = new Disk();
        int fileID = 0;
        for (int i = 0; i < inputs[0].Length; i++)
        {
            int d = inputs[0][i] - '0';
            if (i % 2 == 0)
            {
                disk.AddBlocksForFileOfSize(d, fileID);
                fileID++;
            }
            else
            {
                disk.AddBlocksForEmptyOfSize(d);
            }
        }
        if (debug)
        {
            Console.Out.WriteLine($" -- model     : {inputs[0]}");
            Console.Out.WriteLine($" -- disk      : {disk.ToString()}");
        }

        disk.CompressByIndividualBlock();

        if (debug)
        {
            Console.Out.WriteLine($" -- post comp : {disk.ToString()}");
        }

        checksum = disk.Checksum();

        return "disk checksum = " + checksum;
    }

    public override string Star_2_Impl(string[] inputs, bool debug)
    {
        long checksum = 0;

        Disk disk = new Disk();
        int fileID = 0;
        for (int i = 0; i < inputs[0].Length; i++)
        {
            int d = inputs[0][i] - '0';
            if (i % 2 == 0)
            {
                disk.AddFileRegion(d, fileID);
                fileID++;
            }
            else
            {
                disk.AddEmptyRegion(d);
            }
        }
        if (debug)
        {
            Console.Out.WriteLine($" -- model     : {inputs[0]}");
            Console.Out.WriteLine($" -- disk      : {disk.ToStringRegions()}");
        }

        disk.CompressByRegion(debug);

        if (debug)
        {
            Console.Out.WriteLine($" -- post comp : {disk.ToStringRegions()}");
        }

        checksum = disk.CheckSumByRegion();

        return "disk checksum = " + checksum;
    }
}