
export interface Form<T> {
  id: string;
  data: T;
}
export interface GuildFormData {
  questName: string;
  questType: string;
  color: string;
  location: string;
  questDetails: string | null;
  dangerLevel: number;
  rewardGold: number;
  deadline: string | null;
  requirements: boolean[];
  acceptedGuildCode: boolean;
}

export interface GuildFormProps {
  id?: string | null;
}
