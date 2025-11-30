
export interface Form<T> {
  id: string;
  data: T;
}
export interface FormData {
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

export interface FormDataProps {
  id?: string | null;
}
