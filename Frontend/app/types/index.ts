export interface Release {
  id: string;
  title: string;
  formats: string;
  description: string;
  coverImageUrl: string;
  spotifyUrl?: string;
  appleMusicUrl?: string;
  youtubeUrl?: string;
  releaseDate: string;
  createdAt: string;
}

export interface DiscographyItem {
  id: string;
  title: string;
  externalUrl: string;
  role: string;
  releaseDate: string;
  createdAt: string;
}

export interface DemoTrack {
  id: string;
  name: string;
  genre: string;
  description: string;
  audioUrl: string;
  waveformDataJson?: string;
}

export interface ProfileInfo {
  id: string;
  description: string;
  email: string;
  instagramUrl?: string;
  spotifyUrl?: string;
  soundcloudUrl?: string;
  youtubeUrl?: string;
}