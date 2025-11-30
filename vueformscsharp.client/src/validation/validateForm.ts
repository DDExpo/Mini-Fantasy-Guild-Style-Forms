import type { EmptyForm } from "@/store/forms"

export function validateForm(form: typeof EmptyForm) {
  const errors: Record<string, string> = {}

  if (!form.questName || form.questName.length < 15 || form.questName.length > 190) {
    errors.questName = "Quest name must be between 15 and 190 characters"
  }

  if (!form.location) {
    errors.location = "Location is required"
  }

  if (form.rewardGold <= 0 || form.rewardGold > 4000000000) {
    errors.rewardGold = "Gold must be more than 0 and less than 4,000,000,000"
  }

  if (!form.acceptedGuildCode) {
    errors.acceptedGuildCode = "You must accept the guild code"
  }

  return errors
}
