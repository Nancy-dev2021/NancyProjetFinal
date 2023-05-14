using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Data.SqlClient;

namespace ProjetFinal.Models
{
    public partial class QuizExamenContext : DbContext
    {
        public QuizExamenContext()
        {
        }

        public QuizExamenContext(DbContextOptions<QuizExamenContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Answer> Answer { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<ItemOption> ItemOption { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<QuestionQuiz> QuestionQuiz { get; set; }
        public virtual DbSet<Quiz> Quiz { get; set; }

        //Est-ce que je dois commenter le OnConfiguring comme l'a fait Hind dans sa vidéo d'app Banque?
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Name=MaBdd");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.Property(e => e.AnswerId).HasColumnName("answerID");

                entity.Property(e => e.OptionId).HasColumnName("optionID");

                entity.Property(e => e.QuizId).HasColumnName("quizID");

                entity.HasOne(d => d.Option)
                    .WithMany(p => p.Answer)
                    .HasForeignKey(d => d.OptionId)
                    .HasConstraintName("FK__Answer__optionID__412EB0B6");

                entity.HasOne(d => d.Quiz)
                    .WithMany(p => p.Answer)
                    .HasForeignKey(d => d.QuizId)
                    .HasConstraintName("FK__Answer__quizID__4222D4EF");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryId).HasColumnName("categoryID");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ItemOption>(entity =>
            {
                entity.HasKey(e => e.OptionId)
                    .HasName("PK__ItemOpti__3D5DC3C1951CB36B");

                entity.Property(e => e.OptionId).HasColumnName("optionID");

                entity.Property(e => e.IsRight).HasColumnName("isRight");

                entity.Property(e => e.QuestionId).HasColumnName("questionID");

                entity.Property(e => e.Text)
                    .HasColumnName("text")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.ItemOption)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("FK__ItemOptio__quest__3C69FB99");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.Property(e => e.QuestionId).HasColumnName("questionID");

                entity.Property(e => e.CategoryId).HasColumnName("categoryID");

                entity.Property(e => e.Text)
                    .HasColumnName("text")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Question)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK__Question__catego__398D8EEE");
            });

            modelBuilder.Entity<QuestionQuiz>(entity =>
            {
                entity.HasKey(e => new { e.QuestionId, e.QuizId })
                    .HasName("PK__Question__AEC780531F30DCA7");

                entity.Property(e => e.QuestionId).HasColumnName("questionID");

                entity.Property(e => e.QuizId).HasColumnName("quizID");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.QuestionQuiz)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QuestionQ__quest__44FF419A");

                entity.HasOne(d => d.Quiz)
                    .WithMany(p => p.QuestionQuiz)
                    .HasForeignKey(d => d.QuizId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__QuestionQ__quizI__45F365D3");
            });

            modelBuilder.Entity<Quiz>(entity =>
            {
                entity.Property(e => e.QuizId).HasColumnName("quizID");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .HasColumnName("userName")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
