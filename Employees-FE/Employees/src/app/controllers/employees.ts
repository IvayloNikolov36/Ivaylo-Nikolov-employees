import { environment } from "../../environments/environment";

const route: string = environment.apiUrl + 'Employees/';

export const uploadDataUrl = (): string => route + 'upload';