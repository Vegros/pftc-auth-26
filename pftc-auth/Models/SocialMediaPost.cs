using System.ComponentModel.DataAnnotations;
using Google.Cloud.Firestore;

namespace pftc_auth.Models;

[FirestoreData]
public class SocialMediaPost
{
    [Required]
    [FirestoreProperty]
    public string PostId { get; set; }
    [FirestoreProperty]
    public string PostContent { get; set; }
    [FirestoreProperty]
    public string PostAuthor { get; set; }
    [FirestoreProperty]
    public DateTimeOffset PostDate { get; set; } 
}