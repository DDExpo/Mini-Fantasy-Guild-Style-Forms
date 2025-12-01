import { ref, reactive } from "vue";
import type { GuildFormData, Form } from "@/types/form";

export const forms = ref<Form<GuildFormData>[]>([])

export const FilteringActionsStore = reactive<{
  searchQuery: string;
  sortField: keyof GuildFormData;
  sortOrder: ('asc' | 'desc');
}>({
  searchQuery: '',
  sortField: 'questName',
  sortOrder: 'asc',
})

export const OptionsMainForm = [
  { id: 1, label: "Party Required" },
  { id: 2, label: "Solo Allowed" },
  { id: 3, label: "Mage Required" },
  { id: 4, label: "Knight Required" },
  { id: 5, label: "Healing Items Needed" },
  { id: 6, label: "Night Completion" },
  { id: 7, label: "Stealth Required" },
  { id: 8, label: "No Casualties" },
  { id: 9, label: "Boss Target" },
  { id: 10, label: "Danger Level S+" }
];

export const EmptyForm: GuildFormData = {
  questName: "",
  questType: "Monster Hunt",
  color: "#000000",
  location: "",
  questDetails: "",
  dangerLevel: 1,
  rewardGold: 0,
  requirements: Array(OptionsMainForm.length).fill(false),
  deadline: null,
  acceptedGuildCode: false,
}
export const guildForm = reactive<GuildFormData>({ ...EmptyForm });
