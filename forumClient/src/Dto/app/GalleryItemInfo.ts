import type { ApiFileResponse } from "@/ApiResponses/ApiFileResponse";

export type GalleryItemEntry = {
    galleryItemId: string;
    createdByUserId: string;
    galleryItemName: string;
    galleryItemDescription: string;
    galleryItemLink: string;
    numLikes: number;
    numDislikes: number;
}

export type GalleryItemBasicInfo = {
    galleryItemId: string;
    createdByUserId: string;
    galleryItemName: string;
    galleryItemDescription: string;
    galleryItemLink: string;
    numLikes: number;
    numDislikes: number;
    imageData: ApiFileResponse;
}

export type CreateGalleryItemRequest = {
    galleryItemName: string;
    galleryItemDescription: string;
}

export type DisplayImageData = {
    galleryItemDescription: string;
    galleryItemLink: string;
}

export type GalleryItemSummary = {
    totalGalleryItems: number,
    totalImages: number
    //Other types to be implemented.
}
