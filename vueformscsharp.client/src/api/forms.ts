import type { GuildFormData } from "@/types/form";

export async function fetchAllFormsApi() {
  const response = await fetch(`/api/getAll`, { method: 'GET' })
  return await response.json()
}

export async function submitFormApi(id: string | undefined | null, guildForm: GuildFormData) {
  const url = id ? `/api/change/${id}` : '/api/create';
  const method = id ? 'PUT' : 'POST';

  const response = await fetch(url, {
    method,
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(guildForm),
  });

  const result = await response.json();
  if (!response.ok) {
    throw new Error(result.error || `Failed to ${id ? 'update' : 'create'} form`);
  }

  return result;
}

export async function deleteFormApi(id: string) {
  const response = await fetch(`/api/delete/${id}`, { method: 'DELETE' });
  if (!response.ok) throw Error(`Status ${response.status}`);
  return await response.json();
}
