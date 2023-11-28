import axios, { type AxiosInstance} from "axios";

export abstract class BaseService {
    protected readonly $http: AxiosInstance;
  
    protected constructor(controller: string, timeout: number = 50000) {
      this.$http = axios.create({
        timeout,
        baseURL: `http://localhost:5224/v1/${controller}/`
      });
    }
  }