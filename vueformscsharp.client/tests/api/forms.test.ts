import { describe, it, expect, vi, beforeEach } from 'vitest';
import { fetchAllFormsApi, submitFormApi, deleteFormApi, FormData } from '@/api/forms';

describe('Forms API (mocked)', () => {
  const sampleForm: FormData = {
    questName: 'Test Quest',
    questType: 'Adventure',
    color: 'Red',
    location: 'Forest',
    questDetails: 'Details here',
    dangerLevel: 3,
    rewardGold: 100,
    deadline: null,
    requirements: [true, false],
    acceptedGuildCode: true,
  };

  beforeEach(() => {
    vi.restoreAllMocks();
  });

  it('fetchAllFormsApi returns array', async () => {
    const mockData = [{ id: 1, data: sampleForm }];
    vi.stubGlobal('fetch', vi.fn(() =>
      Promise.resolve({
        ok: true,
        json: () => Promise.resolve(mockData),
      } as Response)
    ));

    const result = await fetchAllFormsApi();
    expect(result).toEqual(mockData);
    expect(fetch).toHaveBeenCalledWith(`/api/getAll`, { method: 'GET' });
  });

  it('submitFormApi sends POST when no id and returns id', async () => {
    const mockId = 42;
    vi.stubGlobal('fetch', vi.fn(() =>
      Promise.resolve({
        ok: true,
        json: () => Promise.resolve(mockId),
      } as Response)
    ));

    const result = await submitFormApi(undefined, sampleForm);
    expect(result).toBe(mockId);
    expect(fetch).toHaveBeenCalledWith(
      '/api/create',
      expect.objectContaining({
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(sampleForm),
      })
    );
  });

  it('submitFormApi sends PUT when id is provided and returns id', async () => {
    const mockId = 42;
    const id = '1';
    vi.stubGlobal('fetch', vi.fn(() =>
      Promise.resolve({
        ok: true,
        json: () => Promise.resolve(mockId),
      } as Response)
    ));

    const result = await submitFormApi(id, sampleForm);
    expect(result).toBe(mockId);
    expect(fetch).toHaveBeenCalledWith(
      `/api/change/${id}`,
      expect.objectContaining({
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(sampleForm),
      })
    );
  });

  it('submitFormApi throws on fetch error', async () => {
    vi.stubGlobal('fetch', vi.fn(() =>
      Promise.resolve({
        ok: false,
        status: 500,
        json: () => Promise.resolve({ error: 'Server error' })
      } as Response)
    ));

    await expect(submitFormApi(undefined, sampleForm))
      .rejects.toThrow();

    await expect(submitFormApi('1', sampleForm))
      .rejects.toThrow();
  });

  it('deleteFormApi calls fetch with correct URL and method', async () => {
    const mockId = '123';
    const mockResponseData = { success: true };

    (fetch as unknown as vi.Mock).mockResolvedValueOnce({
      ok: true,
      json: () => Promise.resolve(mockResponseData),
    } as Response);

    const result = await deleteFormApi(mockId);

    expect(fetch).toHaveBeenCalledWith(`/api/delete/${mockId}`, { method: 'DELETE' });
    expect(result).toEqual(mockResponseData);
  });

  it('deleteFormApi throws an error if response is not ok', async () => {
    const mockId = '123';

    (fetch as unknown as vi.Mock).mockResolvedValueOnce({
      ok: false,
      status: 404,
      json: () => Promise.resolve({}),
    } as Response);

    await expect(deleteFormApi(mockId)).rejects.toThrow('Status 404');
  });
});