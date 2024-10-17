using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiServer.Dto.Admin
{
    public class AdminPortalStats
    {
        public int TotalCategories { get; set; } = 0;
        public int TotalBoards { get; set; } = 0;
        public int TotalUsers { get; set; } = 0;
        public int TotalThreads { get; set; } = 0;
        public int TotalTopics { get; set; } = 0;
        public int TotalPosts { get; set; } = 0;
        public int TotalGalleryItems { get; set; } = 0;
        public int TotalImages { get; set; } = 0;
        public int TotalVideos { get; set; } = 0;
        public int TotalFiles { get; set; } = 0;
        public int TotalAudioFiles { get; set; } = 0;

    }
    /*
    //Note to self: when converting to TS, vars like "TotalImages" should be "totalImages" etc. 
    //This is due to Json serialisation by the backend when sending the data to the frontend
    export type AdminPortalStats = {
        totalCategories: number;
        totalBoards: number;
        totalUsers: number;
        totalThreads: number;
        totalTopics: number;
        totalPosts: number;
        totalGalleryItems: number;
        totalImages: number;
        totalVideos: number;
        totalFiles: number;
        totalAudioFiles: number;
    }
    */
}