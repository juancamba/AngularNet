import { Photo } from "./Photo"


export interface Member {
    id: number
    username: string
    photoUrl: string
    age: number
    knownAs: string
    createdAt: Date
    lastActive: Date
    gender: string
    introduction: string
    lookingForm: string
    interests: string
    city: string
    country: string
    photos: Photo[]
  }
  
