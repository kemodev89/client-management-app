import api from "./index";
export const getClients   = () => api.get("/clients");
export const getClient    = (id) => api.get(`/clients/${id}`);
export const createClient = (p) => api.post("/clients", p);
export const updateClient = (id, p) => api.put(`/clients/${id}`, p);
export const deleteClient = (id) => api.delete(`/clients/${id}`);
