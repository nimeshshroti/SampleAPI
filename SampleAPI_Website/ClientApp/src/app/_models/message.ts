export interface Message {
  id: number;
  senderId: number;
  senderKnownAs: string;
  senderPhotoURL: string;
  receiverId: number;
  receiverKnownAs: string;
  receiverPhotoURL: string;
  content: string;
  isRead: boolean;
  dateRead: Date;
  messageSent: Date;
}
